# PuppetCat

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)

[English](#english) | [中文](#中文)

---

<a name="english"></a>
## English

### Overview

**PuppetCat** is a lightweight, production-ready back-end API development framework built on .NET Core 8.0. It provides a well-structured, layered architecture with built-in middleware for logging, error handling, and routing, making it easy to build robust RESTful APIs quickly.

### Features

- 🚀 **Modern .NET 8.0** - Built on the latest .NET platform for optimal performance
- 🏗️ **Layered Architecture** - Clean separation of concerns with distinct layers (API, WebLogic, Business, Core, Framework)
- 📝 **Comprehensive Logging** - Integrated NLog middleware for request/response tracking
- 🛡️ **Error Handling** - Built-in global error handling middleware with custom exception types
- 📊 **Swagger Integration** - Auto-generated API documentation with Swagger UI
- 🗃️ **Entity Framework Support** - Database abstraction with repository pattern implementation
- 🔄 **Async/Await** - Full support for asynchronous operations
- 🎯 **CORS Support** - Pre-configured CORS policy for cross-origin requests
- 🧰 **Utility Libraries** - Common utilities for string manipulation, entity mapping, versioning, and email
- 📦 **Dependency Injection** - Built-in DI container support for loose coupling

### Technology Stack

- **.NET 8.0** - Core framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **Swashbuckle (Swagger)** - API documentation
- **NLog** - Logging framework
- **Newtonsoft.Json** - JSON serialization
- **MySQL** - Database (sample configuration)

### Project Structure

```
PuppetCat/
├── src/
│   ├── PuppetCat.AspNetCore.Core/          # Core utilities and extensions
│   ├── PuppetCat.AspNetCore.Mvc/           # MVC framework components
│   │   ├── Base/                            # Base classes (Controllers, Request/Response models)
│   │   ├── Filter/                          # Action filters
│   │   ├── Middleware/                      # Custom middleware (Logging, Error handling, Routing)
│   │   └── Log/                             # Logging entities
│   ├── PuppetCat.Sample.API/               # Sample API application
│   ├── PuppetCat.Sample.WebLogic/          # Sample web logic layer
│   ├── PuppetCat.Sample.Core/              # Sample business core
│   ├── PuppetCat.Sample.Repository/        # Sample repository layer
│   └── PuppetCat.Sample.Data/              # Sample data models and DbContext
├── LICENSE
└── README.md
```

### Layer Responsibilities

1. **API Layer** (`PuppetCat.Sample.API`) - Controllers, routing, HTTP request/response handling
2. **WebLogic Layer** (`PuppetCat.Sample.WebLogic`) - Business logic and API models
3. **Business Layer** (`PuppetCat.Sample.Repository`, `PuppetCat.Sample.Data`) - Data access, repositories, and database context
4. **Core Layer** (`PuppetCat.Sample.Core`) - Business-specific core functionality
5. **Framework Layer** (`PuppetCat.AspNetCore.*`) - Reusable framework components

### Getting Started

#### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- MySQL Server (or modify connection string for your preferred database)
- Git

#### Installation

1. **Clone the repository**

```bash
git clone <repository-url>
cd PuppetCat
```

2. **Configure the database connection**

Edit `src/PuppetCat.Sample.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "SampleConnection": "Server=127.0.0.1;database=puppetcat_sample;uid=puppetcat_user;password=YOUR_PASSWORD;"
  }
}
```

3. **Build the solution**

```bash
cd src
dotnet build PuppetCat.sln
```

4. **Run the sample API**

```bash
cd PuppetCat.Sample.API
dotnet run
```

The API will start on `https://localhost:5001` (or the port specified in `launchSettings.json`).

#### Access Swagger UI

Once the application is running, navigate to:

```
https://localhost:5001/swagger
```

This will display the Swagger UI with all available API endpoints.

### Configuration

#### Application Settings

The `appsettings.json` file contains the following configurations:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "SampleConnection": "Your database connection string"
  },
  "AppSettings": {
    "DistributeRoutePath": "/,/api",
    "DistributeRouteIgnorePath": "/Swagger"
  }
}
```

- **DistributeRoutePath**: Routes that will be processed by the distribute route middleware
- **DistributeRouteIgnorePath**: Routes that should bypass the distribute route middleware

#### NLog Configuration

Logging is configured in `nlog.config`. By default, logs are written to files with request/response tracking.

### Usage Examples

#### Creating a Controller

```csharp
[Route("User")]
public class UserController : BaseController
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("GetAll")]
    [ProducesResponseType(typeof(ResponseDefault<List<ApiUserGetAllResponse>>), 200)]
    public async Task<JsonResult> GetAll([FromBody]RequestNoData request)
    {
        List<User> list = await _userRepository.LoadListAllAsync();
        List<ApiUserGetAllResponse> listRes = EntityUtils.CopyToList<User, ApiUserGetAllResponse>(list);
        return CreateResult<List<ApiUserGetAllResponse>>(ResponseStatusCode.OK, string.Empty, listRes);
    }
}
```

#### Response Format

All API responses follow a standard format:

```json
{
  "result": 0,
  "msg": "",
  "data": {},
  "count": 0
}
```

- **result**: Status code (0 = success, others = error codes)
- **msg**: Message or error description
- **data**: Response payload
- **count**: Total count (used for pagination)

### Built-in Middleware

1. **LogHandlingMiddleware** - Logs all POST requests with request/response details
2. **ErrorHandlingMiddleware** - Catches and handles exceptions globally
3. **DistributeRoute** - Custom routing distribution logic

### Development

#### Adding a New Controller

1. Create controller in `PuppetCat.Sample.API/Controllers`
2. Inherit from `BaseController`
3. Use `CreateResult<T>()` method for consistent response formatting
4. Register dependencies in `Startup.cs`

#### Adding a New Repository

1. Create repository in `PuppetCat.Sample.Repository`
2. Inherit from `BaseRepository<T>`
3. Register in `Startup.cs` using `services.AddScoped<YourRepository>()`

### Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Author

**JerryLuo5799**

---

<a name="中文"></a>
## 中文

### 概述

**PuppetCat** 是一个基于 .NET Core 8.0 构建的轻量级、生产就绪的后端 API 开发框架。它提供了良好的分层架构，内置日志记录、错误处理和路由中间件，使您能够快速构建健壮的 RESTful API。

### 特性

- 🚀 **现代化 .NET 8.0** - 基于最新的 .NET 平台构建，性能卓越
- 🏗️ **分层架构** - 清晰的关注点分离，具有不同的层次（API、WebLogic、Business、Core、Framework）
- 📝 **完善的日志记录** - 集成 NLog 中间件进行请求/响应跟踪
- 🛡️ **错误处理** - 内置全局错误处理中间件和自定义异常类型
- 📊 **Swagger 集成** - 自动生成 API 文档和 Swagger UI
- 🗃️ **Entity Framework 支持** - 使用仓储模式实现数据库抽象
- 🔄 **异步支持** - 完全支持异步操作（Async/Await）
- 🎯 **CORS 支持** - 预配置的 CORS 策略用于跨域请求
- 🧰 **实用工具库** - 字符串处理、实体映射、版本控制和邮件等常用工具
- 📦 **依赖注入** - 内置 DI 容器支持松耦合

### 技术栈

- **.NET 8.0** - 核心框架
- **ASP.NET Core** - Web API 框架
- **Entity Framework Core** - 数据库操作 ORM
- **Swashbuckle (Swagger)** - API 文档
- **NLog** - 日志框架
- **Newtonsoft.Json** - JSON 序列化
- **MySQL** - 数据库（示例配置）

### 项目结构

```
PuppetCat/
├── src/
│   ├── PuppetCat.AspNetCore.Core/          # 核心工具类和扩展
│   ├── PuppetCat.AspNetCore.Mvc/           # MVC 框架组件
│   │   ├── Base/                            # 基类（控制器、请求/响应模型）
│   │   ├── Filter/                          # 操作过滤器
│   │   ├── Middleware/                      # 自定义中间件（日志、错误处理、路由）
│   │   └── Log/                             # 日志实体
│   ├── PuppetCat.Sample.API/               # 示例 API 应用
│   ├── PuppetCat.Sample.WebLogic/          # 示例 Web 逻辑层
│   ├── PuppetCat.Sample.Core/              # 示例业务核心层
│   ├── PuppetCat.Sample.Repository/        # 示例仓储层
│   └── PuppetCat.Sample.Data/              # 示例数据模型和 DbContext
├── LICENSE
└── README.md
```

### 层次职责

1. **API 层** (`PuppetCat.Sample.API`) - 控制器、路由、HTTP 请求/响应处理
2. **Web 逻辑层** (`PuppetCat.Sample.WebLogic`) - 业务逻辑和 API 模型
3. **业务层** (`PuppetCat.Sample.Repository`, `PuppetCat.Sample.Data`) - 数据访问、仓储和数据库上下文
4. **核心层** (`PuppetCat.Sample.Core`) - 业务特定的核心功能
5. **框架层** (`PuppetCat.AspNetCore.*`) - 可重用的框架组件

### 快速开始

#### 前置要求

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 或更高版本
- [Visual Studio 2022](https://visualstudio.microsoft.com/) 或 [Visual Studio Code](https://code.visualstudio.com/)
- MySQL 服务器（或修改连接字符串以使用您喜欢的数据库）
- Git

#### 安装步骤

1. **克隆仓库**

```bash
git clone <repository-url>
cd PuppetCat
```

2. **配置数据库连接**

编辑 `src/PuppetCat.Sample.API/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "SampleConnection": "Server=127.0.0.1;database=puppetcat_sample;uid=puppetcat_user;password=您的密码;"
  }
}
```

3. **构建解决方案**

```bash
cd src
dotnet build PuppetCat.sln
```

4. **运行示例 API**

```bash
cd PuppetCat.Sample.API
dotnet run
```

API 将在 `https://localhost:5001` 上启动（或在 `launchSettings.json` 中指定的端口）。

#### 访问 Swagger UI

应用程序运行后，导航到：

```
https://localhost:5001/swagger
```

这将显示包含所有可用 API 端点的 Swagger UI。

### 配置

#### 应用程序设置

`appsettings.json` 文件包含以下配置：

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "SampleConnection": "您的数据库连接字符串"
  },
  "AppSettings": {
    "DistributeRoutePath": "/,/api",
    "DistributeRouteIgnorePath": "/Swagger"
  }
}
```

- **DistributeRoutePath**: 将由分发路由中间件处理的路由
- **DistributeRouteIgnorePath**: 应绕过分发路由中间件的路由

#### NLog 配置

日志记录在 `nlog.config` 中配置。默认情况下，日志会写入文件并跟踪请求/响应。

### 使用示例

#### 创建控制器

```csharp
[Route("User")]
public class UserController : BaseController
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("GetAll")]
    [ProducesResponseType(typeof(ResponseDefault<List<ApiUserGetAllResponse>>), 200)]
    public async Task<JsonResult> GetAll([FromBody]RequestNoData request)
    {
        List<User> list = await _userRepository.LoadListAllAsync();
        List<ApiUserGetAllResponse> listRes = EntityUtils.CopyToList<User, ApiUserGetAllResponse>(list);
        return CreateResult<List<ApiUserGetAllResponse>>(ResponseStatusCode.OK, string.Empty, listRes);
    }
}
```

#### 响应格式

所有 API 响应都遵循标准格式：

```json
{
  "result": 0,
  "msg": "",
  "data": {},
  "count": 0
}
```

- **result**: 状态码（0 = 成功，其他 = 错误代码）
- **msg**: 消息或错误描述
- **data**: 响应负载
- **count**: 总数（用于分页）

### 内置中间件

1. **LogHandlingMiddleware** - 记录所有 POST 请求及其请求/响应详细信息
2. **ErrorHandlingMiddleware** - 全局捕获和处理异常
3. **DistributeRoute** - 自定义路由分发逻辑

### 开发指南

#### 添加新控制器

1. 在 `PuppetCat.Sample.API/Controllers` 中创建控制器
2. 继承自 `BaseController`
3. 使用 `CreateResult<T>()` 方法进行一致的响应格式化
4. 在 `Startup.cs` 中注册依赖项

#### 添加新仓储

1. 在 `PuppetCat.Sample.Repository` 中创建仓储
2. 继承自 `BaseRepository<T>`
3. 在 `Startup.cs` 中使用 `services.AddScoped<YourRepository>()` 注册

### 贡献

欢迎贡献！请随时提交 Pull Request。

1. Fork 本仓库
2. 创建您的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交您的更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启一个 Pull Request

### 许可证

本项目采用 MIT 许可证 - 详情请参阅 [LICENSE](LICENSE) 文件。

### 作者

**JerryLuo5799**
