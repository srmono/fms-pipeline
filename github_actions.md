GitHub Actions is a CI/CD (Continuous Integration/Continuous Deployment) service provided by GitHub that allows you to automate workflows for your repositories. It helps in automating testing, building, and deploying applications.

---

## **Key Concepts in GitHub Actions**
### 1️⃣ **Workflow**
- A workflow is an automated process defined in a `.github/workflows/*.yml` file.
- It contains a series of jobs that run on GitHub-hosted or self-hosted runners.

### 2️⃣ **Events (Triggers)**
- Workflows are triggered by events such as:
  - `push` (When code is pushed to the repository)
  - `pull_request` (On PR creation or update)
  - `schedule` (At specific times using cron syntax)
  - `workflow_dispatch` (Manual trigger)

### 3️⃣ **Jobs**
- A workflow consists of one or more jobs.
- Jobs run on separate virtual machines.
- Example:
  ```yaml
  jobs:
    build:
      runs-on: ubuntu-latest
      steps:
        - name: Checkout Code
          uses: actions/checkout@v4
  ```

### 4️⃣ **Steps**
- Each job has steps that execute tasks.
- Steps can run commands or use pre-built GitHub Actions.

### 5️⃣ **Actions**
- Actions are reusable units in workflows.
- GitHub provides built-in actions, or you can create custom ones.
- Example: `actions/checkout` for fetching the repository.

### 6️⃣ **Runners**
- Machines where jobs are executed.
- GitHub provides hosted runners like:
  - `ubuntu-latest`
  - `windows-latest`
  - `macos-latest`
- You can also set up **self-hosted runners**.

---

## **Basic Workflow Example**
```yaml
name: CI Pipeline

on:
  push:
    branches:
      - main
  pull_request:

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Set up .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'

      - name: Build Application
        run: dotnet build --configuration Release
```

---

## **Environment Variables and Secrets**
- Define variables using `env`:
  ```yaml
  env:
    DATABASE_URL: "mysql://localhost:3306"
  ```
- Store sensitive data in **GitHub Secrets** (`Settings > Secrets`):
  ```yaml
  env:
    DOCKER_PASSWORD: ${{ secrets.DOCKER_PASSWORD }}
  ```

---

## **Artifacts and Caching**
- **Artifacts**: Store and share build results across jobs.
  ```yaml
  - name: Upload Artifact
    uses: actions/upload-artifact@v4
    with:
      name: build-output
      path: ./bin/Release/
  ```
- **Caching Dependencies**:
  ```yaml
  - uses: actions/cache@v4
    with:
      path: ~/.nuget/packages
      key: dotnet-${{ runner.os }}-${{ hashFiles('**/*.csproj') }}
  ```

---

## **Deploying to Docker Hub**
```yaml
name: Docker Build & Push

on:
  push:
    branches:
      - main

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Log in to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKER_USERNAME }}
          password: ${{ secrets.DOCKER_PASSWORD }}

      - name: Build and Push Docker Image
        run: |
          docker build -t myapp:latest .
          docker tag myapp:latest mydockerhubusername/myapp:latest
          docker push mydockerhubusername/myapp:latest
```

---

### **Next Steps**
- Set up **Matrix Builds** (run tests on multiple OS versions).
- Create **Reusable Workflows**.
- Automate **versioning and releases**.

