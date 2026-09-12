# PapelTrail

**PapelTrail** is a cloud-native document management and processing platform designed to demonstrate modern AWS, DevOps, and distributed-system architecture.

Users can upload, manage, share, and track documents through a web application. Uploaded documents are processed asynchronously using an event-driven architecture, with AWS services responsible for storage, workflow orchestration, messaging, and serverless processing.

The project is developed locally using **Floci** to emulate AWS services, allowing the entire AWS-based infrastructure to be developed and tested without requiring a paid AWS environment.

## Objectives

PapelTrail is primarily a learning and portfolio project focused on gaining practical experience with:

* AWS architecture and services
* Kubernetes and container orchestration
* Infrastructure as Code
* Terraform
* CloudFormation
* Event-driven architecture
* Serverless computing
* Distributed systems
* Observability and monitoring
* CI/CD
* Application and infrastructure security

## Core Features

### Document Management

* Upload documents
* Download documents
* Delete documents
* Document metadata
* Document versioning
* Document status tracking
* Search and filtering
* Temporary document sharing links

### Authentication & Authorization

* User registration and authentication
* JWT-based authentication
* Role-based access control
* User-specific document access
* Multi-tenant document isolation
* Least-privilege AWS permissions

### Document Processing

Uploaded documents go through an asynchronous processing pipeline:

```text
Upload
   ↓
S3
   ↓
Event
   ↓
SQS
   ↓
Lambda / Worker
   ↓
Step Functions
   ↓
Document Processing
   ↓
DynamoDB
   ↓
Completed
```

The processing workflow supports:

* File validation
* Metadata extraction
* Document classification
* Processing retries
* Failure handling
* Dead-letter queues
* Processing status tracking
* Audit history

## Event-Driven Architecture

PapelTrail uses events to represent important system actions.

Example events:

```text
DOCUMENT_UPLOADED
DOCUMENT_PROCESSING_STARTED
DOCUMENT_PROCESSING_COMPLETED
DOCUMENT_PROCESSING_FAILED
DOCUMENT_SHARED
DOCUMENT_DOWNLOADED
DOCUMENT_DELETED
```

Events and processing state are stored separately from the primary application data to demonstrate event-driven and distributed-system patterns.

## AWS Services

The project will utilize the following AWS services through Floci:

| Service        | Purpose                             |
| -------------- | ----------------------------------- |
| S3             | Document storage                    |
| Lambda         | Serverless document processing      |
| API Gateway    | REST API entry point                |
| DynamoDB       | Metadata, events, and audit records |
| SQS            | Asynchronous job processing         |
| Step Functions | Document processing workflow        |
| IAM            | Identity and access management      |
| CloudWatch     | Logs and operational monitoring     |
| EventBridge    | Event-driven integrations           |

## Kubernetes

Supporting application services will be containerized using Docker and deployed to a local Kubernetes cluster.

Planned Kubernetes components:

```text
Kubernetes Cluster
│
├── API Service
├── Document Processing Worker
├── PostgreSQL
├── Prometheus
└── Grafana
```

The project will demonstrate:

* Pods
* Deployments
* Services
* Ingress
* ConfigMaps
* Secrets
* Persistent Volumes
* StatefulSets
* Readiness probes
* Liveness probes
* Resource limits
* Horizontal Pod Autoscaling
* Rolling deployments
* Rollbacks
* Network policies

## Database

PostgreSQL will be used for relational application data.

Example data:

```text
Users
Tenants
Roles
Documents
Document Versions
Permissions
Processing Jobs
```

The project will also use DynamoDB for data that benefits from a NoSQL/event-oriented model.

This provides practical experience comparing relational and NoSQL storage patterns and prepares the application for eventual migration to services such as **Amazon RDS**.

## Infrastructure as Code

The infrastructure will be managed using **Terraform** and **AWS CloudFormation**.

Terraform will be used to provision and manage:

```text
S3
DynamoDB
Lambda
API Gateway
SQS
Step Functions
IAM
Kubernetes resources
```

CloudFormation templates will be created for selected AWS components to compare AWS-native Infrastructure as Code with Terraform.

The environment should be reproducible using:

```bash
terraform init
terraform plan
terraform apply
```

and removable using:

```bash
terraform destroy
```

## Observability

Prometheus will collect application and infrastructure metrics.

Example metrics:

```text
http_requests_total
http_request_duration_seconds

documents_uploaded_total
documents_processed_total
documents_failed_total

processing_duration_seconds

queue_messages_processed_total
queue_messages_failed_total

database_connections
```

Grafana will provide dashboards for:

* API performance
* Document processing
* Queue health
* Error rates
* Kubernetes resources
* CPU and memory usage
* Worker activity

The project will also include intentional failure and load-testing scenarios to demonstrate monitoring and troubleshooting.

## Reliability

PapelTrail will implement several reliability patterns:

* Asynchronous processing
* Retry mechanisms
* Dead-letter queues
* Idempotency
* Health checks
* Kubernetes self-healing
* Horizontal scaling
* Database transactions
* Outbox pattern
* Saga/compensating transactions where appropriate
* Graceful failure handling

## Security

Security will be treated as part of the architecture rather than an afterthought.

Planned security controls include:

* JWT authentication
* RBAC
* Tenant isolation
* Input validation
* Rate limiting
* Secure HTTP headers
* Password hashing
* IAM least privilege
* Kubernetes security contexts
* Non-root containers
* Secrets management
* Container vulnerability scanning
* Kubernetes NetworkPolicies

## CI/CD

GitHub Actions will automate the development and deployment workflow.

```text
Git Push
   ↓
Lint
   ↓
Unit Tests
   ↓
Integration Tests
   ↓
Build
   ↓
Docker Build
   ↓
Security Scan
   ↓
Deploy
   ↓
Kubernetes
   ↓
Health Check
```

## Future: Apache Flink

Apache Flink will eventually be introduced to process document and system events in real time.

Potential use cases include:

* Processing activity analytics
* Event aggregation
* Processing-rate monitoring
* Anomaly detection
* Real-time operational metrics
* Event-based alerts

Example:

```text
Document Events
      ↓
Apache Flink
      ↓
Real-Time Analytics
      ↓
Metrics / Alerts
```

## Target Architecture

```text
                         ┌──────────────┐
                         │    React     │
                         │   Frontend   │
                         └──────┬───────┘
                                │
                                ▼
                         ┌──────────────┐
                         │ API Gateway  │
                         └──────┬───────┘
                                │
                                ▼
                     ┌─────────────────────┐
                     │   Application API   │
                     │     Kubernetes      │
                     └──────┬──────────────┘
                            │
              ┌─────────────┼─────────────┐
              ▼             ▼             ▼
         PostgreSQL       S3           DynamoDB
              │             │             │
              │             ▼             │
              │           Events           │
              │             │             │
              │             ▼             │
              │            SQS             │
              │             │              │
              │             ▼              │
              │          Lambda            │
              │             │              │
              │             ▼              │
              │      Step Functions        │
              │             │              │
              └─────────────┼──────────────┘
                            │
                            ▼
                    Processing Workers
                       Kubernetes

              ┌─────────────────────────┐
              │       Observability      │
              │                         │
              │ Prometheus → Grafana    │
              └─────────────────────────┘

              ┌─────────────────────────┐
              │ Infrastructure as Code  │
              │                         │
              │ Terraform / CloudFormation│
              └─────────────────────────┘
```

## Development Philosophy

PapelTrail is intentionally being developed incrementally.

The goal is not simply to use as many technologies as possible. Each technology should solve a specific problem and provide practical experience with production-style architecture.

The project will progressively evolve from a simple document application into a distributed cloud-native platform.

### Development Roadmap

```text
Phase 1
Application + PostgreSQL + Docker
        ↓
Phase 2
Authentication + RBAC
        ↓
Phase 3
S3 + DynamoDB
        ↓
Phase 4
Lambda + API Gateway
        ↓
Phase 5
SQS + Step Functions
        ↓
Phase 6
Kubernetes
        ↓
Phase 7
Prometheus + Grafana
        ↓
Phase 8
Terraform + CloudFormation
        ↓
Phase 9
CI/CD + Security
        ↓
Phase 10
Apache Flink + Real-Time Analytics
```

## Goal

The ultimate goal of PapelTrail is to provide a practical environment for developing and demonstrating skills in **AWS, Kubernetes, Terraform, Docker, PostgreSQL, serverless architecture, event-driven systems, observability, CI/CD, and distributed systems** while keeping the entire development environment locally reproducible.
# PapelTrail

**PapelTrail** is a cloud-native document management and processing platform designed to demonstrate modern AWS, DevOps, and distributed-system architecture.

Users can upload, manage, share, and track documents through a web application. Uploaded documents are processed asynchronously using an event-driven architecture, with AWS services responsible for storage, workflow orchestration, messaging, and serverless processing.

The project is developed locally using **Floci** to emulate AWS services, allowing the entire AWS-based infrastructure to be developed and tested without requiring a paid AWS environment.

## Objectives

PapelTrail is primarily a learning and portfolio project focused on gaining practical experience with:

* AWS architecture and services
* Kubernetes and container orchestration
* Infrastructure as Code
* Terraform
* CloudFormation
* Event-driven architecture
* Serverless computing
* Distributed systems
* Observability and monitoring
* CI/CD
* Application and infrastructure security

## Core Features

### Document Management

* Upload documents
* Download documents
* Delete documents
* Document metadata
* Document versioning
* Document status tracking
* Search and filtering
* Temporary document sharing links

### Authentication & Authorization

* User registration and authentication
* JWT-based authentication
* Role-based access control
* User-specific document access
* Multi-tenant document isolation
* Least-privilege AWS permissions

### Document Processing

Uploaded documents go through an asynchronous processing pipeline:

```text
Upload
   ↓
S3
   ↓
Event
   ↓
SQS
   ↓
Lambda / Worker
   ↓
Step Functions
   ↓
Document Processing
   ↓
DynamoDB
   ↓
Completed
```

The processing workflow supports:

* File validation
* Metadata extraction
* Document classification
* Processing retries
* Failure handling
* Dead-letter queues
* Processing status tracking
* Audit history

## Event-Driven Architecture

PapelTrail uses events to represent important system actions.

Example events:

```text
DOCUMENT_UPLOADED
DOCUMENT_PROCESSING_STARTED
DOCUMENT_PROCESSING_COMPLETED
DOCUMENT_PROCESSING_FAILED
DOCUMENT_SHARED
DOCUMENT_DOWNLOADED
DOCUMENT_DELETED
```

Events and processing state are stored separately from the primary application data to demonstrate event-driven and distributed-system patterns.

## AWS Services

The project will utilize the following AWS services through Floci:

| Service        | Purpose                             |
| -------------- | ----------------------------------- |
| S3             | Document storage                    |
| Lambda         | Serverless document processing      |
| API Gateway    | REST API entry point                |
| DynamoDB       | Metadata, events, and audit records |
| SQS            | Asynchronous job processing         |
| Step Functions | Document processing workflow        |
| IAM            | Identity and access management      |
| CloudWatch     | Logs and operational monitoring     |
| EventBridge    | Event-driven integrations           |

## Kubernetes

Supporting application services will be containerized using Docker and deployed to a local Kubernetes cluster.

Planned Kubernetes components:

```text
Kubernetes Cluster
│
├── API Service
├── Document Processing Worker
├── PostgreSQL
├── Prometheus
└── Grafana
```

The project will demonstrate:

* Pods
* Deployments
* Services
* Ingress
* ConfigMaps
* Secrets
* Persistent Volumes
* StatefulSets
* Readiness probes
* Liveness probes
* Resource limits
* Horizontal Pod Autoscaling
* Rolling deployments
* Rollbacks
* Network policies

## Database

PostgreSQL will be used for relational application data.

Example data:

```text
Users
Tenants
Roles
Documents
Document Versions
Permissions
Processing Jobs
```

The project will also use DynamoDB for data that benefits from a NoSQL/event-oriented model.

This provides practical experience comparing relational and NoSQL storage patterns and prepares the application for eventual migration to services such as **Amazon RDS**.

## Infrastructure as Code

The infrastructure will be managed using **Terraform** and **AWS CloudFormation**.

Terraform will be used to provision and manage:

```text
S3
DynamoDB
Lambda
API Gateway
SQS
Step Functions
IAM
Kubernetes resources
```

CloudFormation templates will be created for selected AWS components to compare AWS-native Infrastructure as Code with Terraform.

The environment should be reproducible using:

```bash
terraform init
terraform plan
terraform apply
```

and removable using:

```bash
terraform destroy
```

## Observability

Prometheus will collect application and infrastructure metrics.

Example metrics:

```text
http_requests_total
http_request_duration_seconds

documents_uploaded_total
documents_processed_total
documents_failed_total

processing_duration_seconds

queue_messages_processed_total
queue_messages_failed_total

database_connections
```

Grafana will provide dashboards for:

* API performance
* Document processing
* Queue health
* Error rates
* Kubernetes resources
* CPU and memory usage
* Worker activity

The project will also include intentional failure and load-testing scenarios to demonstrate monitoring and troubleshooting.

## Reliability

PapelTrail will implement several reliability patterns:

* Asynchronous processing
* Retry mechanisms
* Dead-letter queues
* Idempotency
* Health checks
* Kubernetes self-healing
* Horizontal scaling
* Database transactions
* Outbox pattern
* Saga/compensating transactions where appropriate
* Graceful failure handling

## Security

Security will be treated as part of the architecture rather than an afterthought.

Planned security controls include:

* JWT authentication
* RBAC
* Tenant isolation
* Input validation
* Rate limiting
* Secure HTTP headers
* Password hashing
* IAM least privilege
* Kubernetes security contexts
* Non-root containers
* Secrets management
* Container vulnerability scanning
* Kubernetes NetworkPolicies

## CI/CD

GitHub Actions will automate the development and deployment workflow.

```text
Git Push
   ↓
Lint
   ↓
Unit Tests
   ↓
Integration Tests
   ↓
Build
   ↓
Docker Build
   ↓
Security Scan
   ↓
Deploy
   ↓
Kubernetes
   ↓
Health Check
```

## Future: Apache Flink

Apache Flink will eventually be introduced to process document and system events in real time.

Potential use cases include:

* Processing activity analytics
* Event aggregation
* Processing-rate monitoring
* Anomaly detection
* Real-time operational metrics
* Event-based alerts

Example:

```text
Document Events
      ↓
Apache Flink
      ↓
Real-Time Analytics
      ↓
Metrics / Alerts
```

## Target Architecture

```text
                         ┌──────────────┐
                         │    React     │
                         │   Frontend   │
                         └──────┬───────┘
                                │
                                ▼
                         ┌──────────────┐
                         │ API Gateway  │
                         └──────┬───────┘
                                │
                                ▼
                     ┌─────────────────────┐
                     │   Application API   │
                     │     Kubernetes      │
                     └──────┬──────────────┘
                            │
              ┌─────────────┼─────────────┐
              ▼             ▼             ▼
         PostgreSQL       S3           DynamoDB
              │             │             │
              │             ▼             │
              │           Events           │
              │             │             │
              │             ▼             │
              │            SQS             │
              │             │              │
              │             ▼              │
              │          Lambda            │
              │             │              │
              │             ▼              │
              │      Step Functions        │
              │             │              │
              └─────────────┼──────────────┘
                            │
                            ▼
                    Processing Workers
                       Kubernetes

              ┌─────────────────────────┐
              │       Observability      │
              │                         │
              │ Prometheus → Grafana    │
              └─────────────────────────┘

              ┌─────────────────────────┐
              │ Infrastructure as Code  │
              │                         │
              │ Terraform / CloudFormation│
              └─────────────────────────┘
```

## Development Philosophy

PapelTrail is intentionally being developed incrementally.

The goal is not simply to use as many technologies as possible. Each technology should solve a specific problem and provide practical experience with production-style architecture.

The project will progressively evolve from a simple document application into a distributed cloud-native platform.

### Development Roadmap

```text
Phase 1
Application + PostgreSQL + Docker
        ↓
Phase 2
Authentication + RBAC
        ↓
Phase 3
S3 + DynamoDB
        ↓
Phase 4
Lambda + API Gateway
        ↓
Phase 5
SQS + Step Functions
        ↓
Phase 6
Kubernetes
        ↓
Phase 7
Prometheus + Grafana
        ↓
Phase 8
Terraform + CloudFormation
        ↓
Phase 9
CI/CD + Security
        ↓
Phase 10
Apache Flink + Real-Time Analytics
```

## Goal

The ultimate goal of PapelTrail is to provide a practical environment for developing and demonstrating skills in **AWS, Kubernetes, Terraform, Docker, PostgreSQL, serverless architecture, event-driven systems, observability, CI/CD, and distributed systems** while keeping the entire development environment locally reproducible.
