import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { BasketService } from '../../core/services/basket/basket.service';
import { AuthService } from '../../core/services/auth.service';
import { CustomerBasket, BasketCheckout } from '../../core/models/basket/basket.model';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {
  checkoutForm!: FormGroup;
  basket: CustomerBasket | null = null;
  basketTotal = 0;
  isSubmitting = false;
  submitError = '';

  constructor(
    private fb: FormBuilder,
    private basketService: BasketService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadBasketData();
    this.prefillUserData();
  }

  private initForm(): void {
    this.checkoutForm = this.fb.group({
      // Billing Address
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      emailAddress: ['', [Validators.required, Validators.email]],
      addressLine: ['', Validators.required],
      country: ['', Validators.required],
      zipCode: ['', Validators.required],
      // Payment
      cardName: ['', Validators.required],
      cardNumber: ['', [Validators.required, Validators.pattern(/^\d{16}$/)]],
      expiration: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/)]],
      cvv: ['', [Validators.required, Validators.pattern(/^\d{3,4}$/)]],
      paymentMethod: [0, Validators.required]
    });
  }

  private loadBasketData(): void {
    this.basketService.basket$
      .subscribe(basket => {
        this.basket = basket;
        if (basket) {
          this.basketTotal = basket.items.reduce((total, item) => total + (item.price * item.quantity), 0);
        } else {
          this.basketTotal = 0;
        }
      });
  }

  private prefillUserData(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.checkoutForm.patchValue({
        emailAddress: user.email || ''
      });
    }
  }

  onSubmit(): void {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    if (!this.basket || this.basket.items.length === 0) {
      this.submitError = 'Your basket is empty.';
      return;
    }

    const user = this.authService.getCurrentUser();
    if (!user) {
      this.submitError = 'You must be logged in to checkout.';
      return;
    }

    this.isSubmitting = true;
    this.submitError = '';

    const formValues = this.checkoutForm.value;
    const checkoutPayload: BasketCheckout = {
      userName: user.username,
      totalPrice: this.basketTotal,
      firstName: formValues.firstName,
      lastName: formValues.lastName,
      emailAddress: formValues.emailAddress,
      addressLine: formValues.addressLine,
      country: formValues.country,
      zipCode: formValues.zipCode,
      cardName: formValues.cardName,
      cardNumber: formValues.cardNumber,
      expiration: formValues.expiration,
      cvv: formValues.cvv,
      paymentMethod: Number(formValues.paymentMethod)
    };

    this.basketService.checkout(checkoutPayload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/orders']);
      },
      error: (err) => {
        console.error('Checkout failed', err);
        this.submitError = 'Checkout failed. Please try again later.';
        this.isSubmitting = false;
      }
    });
  }

  // Helper for template validation checks
  isFieldInvalid(fieldName: string): boolean {
    const field = this.checkoutForm.get(fieldName);
    return !!field && field.invalid && (field.dirty || field.touched);
  }
}
