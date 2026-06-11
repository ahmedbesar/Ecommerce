# Ecommerce Microservices

This repository is a dockerized ecommerce system built with .NET, Angular, Ocelot, RabbitMQ, gRPC, and multiple databases. It is designed as a learning and portfolio project that shows a real microservices flow: users browse products, sign in, manage a basket, checkout, and have the order created asynchronously.

## What The System Does

The application is split into small services instead of one large backend:

- The Angular UI is the customer-facing web app.
- Ocelot API Gateway is the single backend entry point used by the UI for most API calls.
- Identity API authenticates users and issues OpenIddict access and refresh tokens.
- Catalog API manages products, brands, and product types.
- Discount API stores coupon/discount data and exposes it through gRPC.
- Basket API stores a user's shopping basket in Redis.
- Ordering API stores orders in SQL Server and consumes checkout events from RabbitMQ.
- Shared infrastructure projects provide authentication, logging, and integration event contracts.

## Technology Stack

### Backend

- .NET 10 Web API
- ASP.NET Core Controllers
- Clean Architecture style per service: API, Application, Core, Infrastructure
- CQRS with MediatR
- FluentValidation
- FluentResults
- Entity Framework Core
- Dapper
- MassTransit with RabbitMQ
- gRPC
- OpenIddict and ASP.NET Core Identity
- Ocelot API Gateway
- Swagger/OpenAPI
- Serilog with Elasticsearch sink

### Frontend

- Angular 21
- TypeScript
- RxJS
- Angular Router
- Angular Signals
- Bootstrap 5
- Font Awesome
- Vitest test setup

### Data And Infrastructure

- MongoDB for catalog data
- Redis for basket cache
- PostgreSQL for Identity and Discount databases
- SQL Server for orders
- RabbitMQ for async checkout events
- Elasticsearch and Kibana for centralized logs
- Portainer for Docker container management
- Docker and Docker Compose for local orchestration

## High-Level Architecture

```text
Angular UI (localhost:4200)
        |
        | Login / refresh token
        v
Identity API (localhost:8011) ---- PostgreSQL / IdentityDb
        |
        | Access token
        v
Ocelot API Gateway (localhost:8010)
        |
        +--> Catalog API (localhost:8000) ---- MongoDB / CatalogDb
        |          |
        |          +-- gRPC --> Discount API (localhost:5002) ---- PostgreSQL / DiscountDb
        |
        +--> Basket API (localhost:8001) ---- Redis
        |          |
        |          +-- publishes BasketCheckoutEvent --> RabbitMQ
        |
        +--> Ordering API (localhost:8003) <-- consumes BasketCheckoutEvent
                   |
                   +---- SQL Server / orderDB

All backend services write logs to console and Elasticsearch.
Kibana reads logs from Elasticsearch.
```

## Services

### Angular UI

Location: `ecommerce-ui`

The UI provides pages for home, product list, product details, login, basket, checkout, and orders. It calls Identity API directly for tokens and calls the rest of the backend through Ocelot.

Important routes:

- `/home`
- `/products`
- `/products/:id`
- `/login`
- `/basket`
- `/checkout`
- `/orders`

Protected pages use the auth guard. The access token is stored in session storage and attached to API calls through the auth interceptor.

### Ocelot API Gateway

Location: `ApiGateway/Ocelot.ApiGateway`

The gateway exposes a single public backend URL: `http://localhost:8010`.

It routes:

- `/Products` and `/Products/{everything}` to Catalog API
- `/Brands` and `/Brands/{everything}` to Catalog API
- `/Types` and `/Types/{everything}` to Catalog API
- `/Basket` and `/Basket/{everything}` to Basket API
- `/api/Orders` and `/api/Orders/{everything}` to Ordering API

Basket and Ordering routes require OpenIddict token validation.

### Identity API

Location: `services/Identity/Identity.Api`

Identity API uses ASP.NET Core Identity and OpenIddict. It creates the development client, roles, and demo users during startup.

Token endpoint:

- `POST http://localhost:8011/connect/token`

Seeded development users:

- User: `bob` / `User123!`
- Admin: `admin` / `Admin123!`

Development OpenIddict client:

- Client ID: `ecommerce_dev`
- Client secret: `dev-secret-change-me`
- Scope: `openid profile email roles ecommerce.api offline_access`

These credentials are for local development only.

### Catalog API

Location: `services/catalog`

Catalog API manages products, brands, and types. It stores catalog data in MongoDB and seeds products, brands, and types from JSON seed files.

Main endpoints:

- `GET /Products`
- `GET /Products/{id}`
- `GET /Products/name/{name}`
- `GET /Products/brand/{brandName}`
- `POST /Products`
- `PUT /Products`
- `DELETE /Products/{id}`
- `DELETE /Products/delete-all`
- `GET /Brands`
- `GET /Types`

When product details are requested, Catalog calls Discount API through gRPC and returns discount information with the product response.

### Discount API

Location: `services/Discount`

Discount API owns product coupon data. It stores discounts in PostgreSQL and exposes the discount contract through gRPC on port `5002`.

gRPC operations:

- `GetDiscount`
- `CreateDiscount`
- `UpdateDiscount`
- `DeleteDiscount`

The REST/Swagger endpoint runs on port `8002`, and the gRPC endpoint runs on port `5002`.

### Basket API

Location: `services/Basket`

Basket API stores the current user's basket in Redis. Basket endpoints are protected with JWT/OpenIddict validation and user ownership policies, so a normal user can only access their own basket while an admin can access more.

Main endpoints:

- `GET /Basket/{userName}`
- `POST /Basket`
- `PUT /Basket`
- `POST /Basket/checkout`
- `DELETE /Basket/{userName}`

On checkout, Basket API:

1. Validates the current user.
2. Loads the basket from Redis.
3. Maps checkout data to `BasketCheckoutEvent`.
4. Publishes the event to RabbitMQ using MassTransit.
5. Deletes the basket after publishing.

### Ordering API

Location: `services/Ordering`

Ordering API stores orders in SQL Server using EF Core. It also consumes checkout events from RabbitMQ.

Main endpoints:

- `GET /api/Orders`
- `GET /api/Orders/{userName}`
- `POST /api/Orders`
- `PUT /api/Orders`
- `DELETE /api/Orders/{id}`

Ordering subscribes to the `Basketcheckout-queue` queue. When it receives `BasketCheckoutEvent`, it creates an order through the MediatR command pipeline.

### Shared Infrastructure

Location: `Infrastructure`

Shared projects:

- `Common.Authentication`: OpenIddict validation, shared auth scheme, admin policy, and self-user-or-admin policy.
- `Common.Logging`: Serilog setup with console and Elasticsearch logging.
- `EventBus.Messages`: shared integration event contracts and RabbitMQ queue constants.

## Full User Scenario

1. The user opens the Angular app at `http://localhost:4200`.
2. The UI loads product data from Ocelot at `http://localhost:8010`.
3. Ocelot forwards product requests to Catalog API.
4. Catalog API reads products from MongoDB.
5. When a product detail is loaded, Catalog API calls Discount API through gRPC to calculate discount data.
6. The user signs in through Identity API.
7. Identity API validates credentials and returns access and refresh tokens.
8. The UI stores the session and sends the access token on protected API calls.
9. The user adds products to the basket.
10. Basket API validates the token and stores basket data in Redis.
11. The user submits checkout details.
12. Basket API publishes `BasketCheckoutEvent` to RabbitMQ and clears the Redis basket.
13. Ordering API consumes the event from RabbitMQ.
14. Ordering API creates the final order in SQL Server.
15. Logs from the services are written to console and Elasticsearch, then can be viewed in Kibana.

## Ports

| Component | URL / Port |
| --- | --- |
| Angular UI | `http://localhost:4200` |
| Ocelot API Gateway | `http://localhost:8010` |
| Catalog API | `http://localhost:8000` |
| Basket API | `http://localhost:8001` |
| Discount API REST/Swagger | `http://localhost:8002` |
| Discount API gRPC | `http://localhost:5002` |
| Ordering API | `http://localhost:8003` |
| Identity API | `http://localhost:8011` |
| MongoDB | `localhost:27017` |
| Redis | `localhost:6379` |
| PostgreSQL | `localhost:5432` |
| SQL Server | `localhost:1433` |
| RabbitMQ | `localhost:5672` |
| RabbitMQ Management | `http://localhost:15672` |
| Elasticsearch | `http://localhost:9200` |
| Kibana | `http://localhost:5601` |
| Portainer | `http://localhost:9000` |
| pgAdmin | `http://localhost:5050` |

## Dockerization

Every backend service has its own Dockerfile:

- `services/catalog/Catalog.Api/Dockerfile`
- `services/Basket/Basket.Api/Dockerfile`
- `services/Discount/Discount.Api/Dockerfile`
- `services/Identity/Identity.Api/Dockerfile`
- `services/Ordering/Ordering.Api/Dockerfile`
- `ApiGateway/Ocelot.ApiGateway/Dockerfile`

Docker Compose builds and runs the full backend platform:

- Backend APIs
- API Gateway
- MongoDB
- Redis
- PostgreSQL
- SQL Server
- RabbitMQ
- Elasticsearch
- Kibana
- Portainer
- pgAdmin

Named Docker volumes keep local development data between container restarts:

- `catalogdata`
- `discountdata`
- `orderdata`
- `rabbitmq_data`
- `elasticsearch-data`
- `portainer_data`
- `pgadmin_data`

## Run The Project

### Prerequisites

- Docker Desktop
- .NET 10 SDK
- Node.js and npm
- Angular CLI, or run Angular through npm scripts

### Start The Backend With Docker Compose

From the repository root:

```bash
docker compose up -d --build
```

Check running containers:

```bash
docker compose ps
```

Stop the backend:

```bash
docker compose down
```

Stop the backend and remove local volumes:

```bash
docker compose down -v
```

### Start The Angular UI

```bash
cd ecommerce-ui
npm install
npm start
```

Open:

```text
http://localhost:4200
```

## Useful Development Commands

Restore and build the .NET solution:

```bash
dotnet restore Ecommerce.slnx
dotnet build Ecommerce.slnx
```

Run Angular tests:

```bash
cd ecommerce-ui
npm test
```

Build Angular:

```bash
cd ecommerce-ui
npm run build
```

View Docker logs for one service:

```bash
docker compose logs -f catalog.api
docker compose logs -f basket.api
docker compose logs -f ordering.api
```

## Authentication Example

Request a token:

```bash
curl -X POST http://localhost:8011/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=ecommerce_dev" \
  -d "client_secret=dev-secret-change-me" \
  -d "username=bob" \
  -d "password=User123!" \
  -d "scope=openid profile email roles ecommerce.api offline_access"
```

Use the returned `access_token` as a bearer token for protected gateway routes:

```bash
curl http://localhost:8010/Basket/bob \
  -H "Authorization: Bearer ACCESS_TOKEN_HERE"
```

## Project Structure

```text
ApiGateway/
  Ocelot.ApiGateway/

Infrastructure/
  Common.Authentication/
  Common.Logging/
  EventBus.Messages/

services/
  Basket/
    Basket.Api/
    Basket.Application/
    Basket.Core/
    Basket.Infrastructure/
  catalog/
    Catalog.Api/
    Catalog.Application/
    Catalog.Core/
    Catalog.Infrastructure/
  Discount/
    Discount.Api/
    Discount.Application/
    Discount.Core/
    Discount.Infrastructure/
  Identity/
    Identity.Api/
  Ordering/
    Ordering.Api/
    Ordering.Application/
    Ordering.Core/
    Ordering.Infrastructure/

ecommerce-ui/
docker-compose.yml
docker-compose.override.yml
Ecommerce.slnx
```

## Important Notes

- The compose file contains development credentials only.
- `IdentityDb` and `DiscountDb` share the PostgreSQL server container but use separate databases.
- Ordering runs EF Core migrations and seeds data at startup.
- Identity creates the database if needed, runs migrations, and seeds demo users at startup.
- Catalog seed files are copied to the output and inserted into MongoDB when the product, brand, or type collections are empty.
- Basket and Ordering routes require a valid OpenIddict access token.
- Ocelot is the preferred entry point for UI/backend traffic, while Identity is called directly for token operations.

## Review Summary

This project demonstrates a complete microservices ecommerce scenario with:

- API Gateway routing
- Token-based authentication and authorization
- Database-per-service style persistence
- Redis caching for basket state
- gRPC service-to-service communication
- RabbitMQ asynchronous messaging
- Centralized logging with Elasticsearch and Kibana
- Dockerized local development environment
- Angular frontend consuming the backend flow
