import { signal, computed, Signal } from '@angular/core';

export interface ViewState<T> {
  data: Signal<T>;
  loading: Signal<boolean>;
  error: Signal<string | null>;
  
  setLoading: () => void;
  setSuccess: (data: T) => void;
  setError: (error: string) => void;
}

export function createViewState<T>(initialValue: T): ViewState<T> {
  const state = signal<{ data: T; loading: boolean; error: string | null }>({
    data: initialValue,
    loading: true,
    error: null
  });

  return {
    data: computed(() => state().data),
    loading: computed(() => state().loading),
    error: computed(() => state().error),
    
    setLoading: () => state.update(s => ({ ...s, loading: true, error: null })),
    setSuccess: (newData: T) => state.set({ data: newData, loading: false, error: null }),
    setError: (errorMessage: string) => state.update(s => ({ ...s, loading: false, error: errorMessage }))
  };
}
