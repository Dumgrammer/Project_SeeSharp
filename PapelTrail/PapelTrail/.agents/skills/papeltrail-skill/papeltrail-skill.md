# PapelTrail C# / ASP.NET Core Development Skill

## Purpose

Build **PapelTrail**, a cloud-native document management and processing platform, using C# and ASP.NET Core.

The project is intended to teach C#/.NET, backend architecture, SQL Server, REST APIs, Docker, AWS, Kubernetes, observability, IaC, and CI/CD through one progressively expanding application.

Do not build the entire platform at once. Work incrementally and keep every phase runnable.

---

## Core Stack

- Backend: C# + ASP.NET Core Web API
- Architecture: MVC-inspired layered architecture
- ORM: Entity Framework Core
- Database: Microsoft SQL Server
- Frontend: React + TypeScript
- Containers: Docker
- Cloud: AWS
- Local AWS practice: Floci
- CI/CD: GitHub Actions
- IaC: Terraform + CloudFormation
- Orchestration: Kubernetes
- Metrics: Prometheus
- Dashboards: Grafana

### Planned AWS services

- S3
- API Gateway
- Lambda
- DynamoDB
- SQS
- Step Functions
- IAM
- CloudWatch
- EventBridge

Apache Flink is a later-stage addition for event-stream analytics.

---

# Architecture

Use this general request flow:

```text
React
  |
  | HTTP / REST
  v
ASP.NET Core Web API
  |
  +--> Controllers
  |
  +--> Services
  |
  +--> EF Core
  |
  v
SQL Server

ASP.NET Core
  |
  +--> S3
  +--> SQS
  +--> Lambda
  +--> Step Functions
  +--> DynamoDB
```

Keep controllers thin.

Business logic belongs in services.

Database access belongs behind EF Core/data-access abstractions where appropriate.

Do not put business logic directly inside controllers.

---

# Project Structure

Prefer a structure similar to:

```text
PapelTrail/
├── Controllers/
│   ├── AuthController.cs
│   ├── DocumentsController.cs
│   ├── UsersController.cs
│   └── AdminController.cs
│
├── Models/
│   ├── User.cs
│   ├── Document.cs
│   ├── DocumentVersion.cs
│   ├── DocumentShare.cs
│   ├── ProcessingJob.cs
│   └── AuditLog.cs
│
├── Data/
│   └── PapelTrailDbContext.cs
│
├── Services/
│   ├── DocumentService.cs
│   ├── StorageService.cs
│   ├── ProcessingService.cs
│   └── AuditService.cs
│
├── DTOs/
│   ├── Documents/
│   ├── Auth/
│   └── Users/
│
├── Middleware/
├── Validators/
├── Migrations/
└── Program.cs
```

The exact structure can evolve as the application grows.

---

# SQL Server Data Model

Use **SQL Server**, not PostgreSQL.

The initial relational model is:

```text
Users
  |
  | 1:N
  v
Documents
  |
  | 1:N
  v
DocumentVersions
  |
  | 1:N
  v
ProcessingJobs

Documents
  |
  | 1:N
  v
DocumentShares

Users
  |
  | 1:N
  v
AuditLogs
```

## User

Represents an authenticated PapelTrail user.

Important fields:

```text
Id
Username
Email
PasswordHash
Role
CreatedAt
```

Use a `Guid`/`uniqueidentifier` as the primary key.

Do not store plaintext passwords.

---

## Document

Represents the logical document rather than the physical file.

Suggested fields:

```text
Id                  uniqueidentifier PK
Name                nvarchar
Description         nvarchar
ContentType         nvarchar
FileSize            bigint
OwnerId             uniqueidentifier FK -> Users
CreatedAt           datetime2
UpdatedAt           datetime2
IsDeleted           bit
```

A document can have multiple versions.

Example:

```text
Project Proposal
├── Version 1
├── Version 2
└── Version 3
```

---

## DocumentVersion

Represents a specific uploaded file version.

Suggested fields:

```text
Id                  uniqueidentifier PK
DocumentId          uniqueidentifier FK -> Documents
VersionNumber       int
StorageKey          nvarchar
FileName            nvarchar
ContentType         nvarchar
FileSize            bigint
Checksum            nvarchar nullable
UploadedById        uniqueidentifier FK -> Users
CreatedAt           datetime2
```

`StorageKey` identifies where the binary file is stored.

For AWS:

```text
documents/{documentId}/v1/...
documents/{documentId}/v2/...
```

The database should not be used as the primary storage location for large document binaries.

---

## DocumentShare

Represents sharing a document with another user.

Suggested fields:

```text
Id                  uniqueidentifier PK
DocumentId          uniqueidentifier FK -> Documents
SharedWithUserId    uniqueidentifier FK -> Users
Permission          nvarchar
CreatedAt           datetime2
ExpiresAt           datetime2 nullable
```

Possible permissions:

```text
View
Edit
```

Later this can evolve into a proper permission model.

---

## ProcessingJob

Represents asynchronous processing of a document version.

Suggested fields:

```text
Id                  uniqueidentifier PK
DocumentId          uniqueidentifier FK -> Documents
DocumentVersionId   uniqueidentifier FK -> DocumentVersions
Status              nvarchar
ErrorMessage        nvarchar nullable
CreatedAt           datetime2
StartedAt           datetime2 nullable
CompletedAt         datetime2 nullable
```

Possible statuses:

```text
Pending
Processing
Completed
Failed
```

Later this model connects to:

```text
S3
  ↓
Event
  ↓
SQS
  ↓
Worker / Lambda
  ↓
Step Functions
  ↓
ProcessingJob
```

---

## AuditLog

Records important actions.

Suggested fields:

```text
Id                  uniqueidentifier PK
UserId              uniqueidentifier nullable FK -> Users
Action              nvarchar
EntityType          nvarchar
EntityId            uniqueidentifier nullable
Details             nvarchar nullable
CreatedAt           datetime2
```

Examples:

```text
DOCUMENT_CREATED
DOCUMENT_UPLOADED
DOCUMENT_VERSION_CREATED
DOCUMENT_SHARED
DOCUMENT_DOWNLOADED
DOCUMENT_DELETED
DOCUMENT_PROCESSING_STARTED
DOCUMENT_PROCESSING_COMPLETED
DOCUMENT_PROCESSING_FAILED
```

---

# EF Core Rules

Use Entity Framework Core with SQL Server.

Configure relationships explicitly when useful rather than relying entirely on conventions.

Example:

```csharp
builder.HasOne(d => d.Owner)
    .WithMany(u => u.Documents)
    .HasForeignKey(d => d.OwnerId)
    .OnDelete(DeleteBehavior.Restrict);
```

Avoid accidental cascade-delete chains.

Use migrations for schema changes.

Typical workflow:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Never manually modify an already-applied migration to change production schema history.

Create a new migration for subsequent schema changes.

---

# SQL Server Development

For local development, SQL Server can run in Docker.

Example conceptual setup:

```text
Docker Compose
├── PapelTrail-api
└── sqlserver
```

Use environment variables for the connection string.

Do not commit database passwords or secrets.

Example configuration concept:

```text
ConnectionStrings__DefaultConnection
```

Use SQL Server-compatible types through EF Core.

Prefer:

- `uniqueidentifier` for GUID IDs
- `nvarchar` for strings
- `bigint` for file sizes
- `int` for version numbers
- `datetime2` for timestamps
- `bit` for booleans

---

# API Design

Use RESTful endpoints.

Initial document API:

```http
POST   /api/documents
GET    /api/documents
GET    /api/documents/{id}
DELETE /api/documents/{id}
```

Later:

```http
POST   /api/documents/{id}/versions
GET    /api/documents/{id}/versions
GET    /api/documents/{id}/versions/{version}
POST   /api/documents/{id}/shares
DELETE /api/documents/{id}/shares/{shareId}
GET    /api/documents/{id}/processing
```

Use DTOs instead of exposing EF Core entities directly from the API.

Validate incoming requests.

Return appropriate HTTP status codes.

Examples:

```text
200 OK
201 Created
204 No Content
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

---

# Security

Security is a core feature, not an afterthought.

Planned security features:

- JWT authentication
- Password hashing
- Role-based access control
- Authorization policies
- Resource-level document authorization
- Input validation
- File type validation
- File size limits
- Secure temporary download links
- Least-privilege AWS IAM
- Secrets outside source control
- Audit logging
- HTTPS
- Security headers where appropriate

Never trust a user-provided `OwnerId` to determine authorization.

Authorization must be based on the authenticated identity.

---

# Document Processing Architecture

The long-term processing pipeline is:

```text
User
 |
 | Upload
 v
ASP.NET Core
 |
 v
S3
 |
 | event
 v
SQS
 |
 v
Worker / Lambda
 |
 v
Step Functions
 |
 +--> Validate
 +--> Virus Scan
 +--> Extract Metadata
 +--> Extract Text
 +--> Generate Thumbnail
 +--> Index
 |
 v
Processing Complete
```

Use asynchronous processing instead of making the upload HTTP request wait for every processing operation.

Implement:

- retries
- idempotency
- dead-letter queues
- failure handling
- processing status
- structured logging

---

# Database vs AWS Storage

SQL Server stores:

```text
Users
Documents
DocumentVersions metadata
Shares
ProcessingJobs
Application data
```

S3 stores:

```text
Actual document files
Generated thumbnails
Other large binary objects
```

DynamoDB can later store:

```text
High-volume event data
Audit/event streams
Processing events
```

Do not duplicate the same source of truth unnecessarily.

---

# Development Roadmap

## Phase 1 — C# Fundamentals + API

Build:

```text
ASP.NET Core
Controllers
Models
Dependency Injection
REST API
EF Core
SQL Server
```

Get basic document CRUD working.

Do not introduce AWS yet.

---

## Phase 2 — Document Versions

Add:

```text
DocumentVersion
Version numbering
Version retrieval
Version history
```

---

## Phase 3 — Authentication

Add:

```text
Registration
Login
JWT
Password hashing
Authorization
Roles
```

---

## Phase 4 — File Storage

Replace local file storage with:

```text
AWS S3
```

During local development, use Floci where practical.

---

## Phase 5 — Async Processing

Introduce:

```text
SQS
Lambda / Worker
Step Functions
ProcessingJob
```

Implement retries and DLQs.

---

## Phase 6 — Kubernetes

Containerize:

```text
ASP.NET Core API
SQL Server
Workers
```

Learn:

```text
Pods
Deployments
Services
Ingress
ConfigMaps
Secrets
Probes
Resource limits
HPA
Rolling deployments
Rollbacks
NetworkPolicies
```

---

## Phase 7 — Observability

Add:

```text
Prometheus
Grafana
Structured logging
Health checks
Application metrics
```

Track useful metrics such as:

```text
HTTP request rate
HTTP error rate
Request latency
Processing jobs
Failed processing jobs
Queue depth
Database performance
```

---

## Phase 8 — Infrastructure as Code

Implement infrastructure using:

```text
Terraform
CloudFormation
```

Prefer understanding the resources being created rather than blindly generating IaC.

---

## Phase 9 — CI/CD

GitHub Actions pipeline:

```text
Push
 ↓
Build
 ↓
Test
 ↓
Lint / Validate
 ↓
Build Docker image
 ↓
Security checks
 ↓
Deploy
```

Add environment separation later.

---

## Phase 10 — Event Analytics

Eventually introduce Apache Flink.

Example:

```text
Document Events
      ↓
Streaming Platform
      ↓
Flink
      ↓
Real-time analytics
```

Possible analytics:

```text
documents uploaded per minute
processing latency
failure rates
download activity
storage activity
user activity
```

---

# Rules for the Coding Agent

1. **Do not implement the entire roadmap at once.**
2. Always work on the smallest useful increment.
3. Explain unfamiliar C#/.NET concepts while implementing them.
4. Prefer production-style architecture over tutorial shortcuts.
5. Keep controllers thin.
6. Put business logic in services.
7. Use DTOs for API contracts.
8. Use dependency injection.
9. Use EF Core migrations for SQL Server schema changes.
10. Never hardcode secrets.
11. Never store plaintext passwords.
12. Do not expose EF Core entities unnecessarily.
13. Add validation at API boundaries.
14. Handle errors consistently.
15. Add tests as important functionality is introduced.
16. Do not add AWS services before the local application is working.
17. Do not add Kubernetes before the Dockerized application works.
18. Do not add Terraform/CloudFormation before the infrastructure is understood manually.
19. Prefer simple implementations first, then increase complexity.
20. When making architectural changes, explain why the change is useful.

---

# Current Starting Point

The first implementation target is:

```text
ASP.NET Core Web API
        ↓
DocumentsController
        ↓
DocumentService
        ↓
EF Core
        ↓
SQL Server
```

Implement only:

```http
POST   /api/documents
GET    /api/documents
GET    /api/documents/{id}
DELETE /api/documents/{id}
```

At this stage, do not add:

```text
AWS
Kubernetes
Terraform
Prometheus
Grafana
SQS
Lambda
Step Functions
Flink
```

First make the foundation work.

Then expand PapelTrail one layer at a time.
