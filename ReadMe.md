# Shimakaze.MinimalApi.Plus

一个用于简化 .NET Minimal API 开发的工具库，通过 Source Generator 提供更优雅的 API 端点定义方式。

||
|:-|
|[![Shimakaze.MinimalApi.Plus](https://img.shields.io/nuget/v/Shimakaze.MinimalApi.Plus?label=Shimakaze.MinimalApi.Plus)](https://www.nuget.org/packages/Shimakaze.MinimalApi.Plus)|
|[![Shimakaze.MinimalApi.Plus.SourceGenerator](https://img.shields.io/nuget/v/Shimakaze.MinimalApi.Plus.SourceGenerator?label=Shimakaze.MinimalApi.Plus.SourceGenerator)](https://www.nuget.org/packages/Shimakaze.MinimalApi.Plus.SourceGenerator)|

## 特性

- 基于 Source Generator 的编译时代码生成
- 支持依赖注入
- 类型安全的路由定义
- 简洁的 API 端点组织方式
- AOT 兼容（.NET 9.0 及以上版本）

## 快速开始

### 1. 安装

```bash
dotnet add package Shimakaze.MinimalApi.Plus
dotnet add package Shimakaze.MinimalApi.Plus.SourceGenerator
```

### 2. 创建 API 端点

```csharp
[ApiEndpoints]
public sealed class SimpleApiEndpoints(SimpleServices services) : ApiEndpoints
{
    [HttpGet("/weatherforecast")]
    public WeatherForecast[] GetWeatherForecasts()
        => services.GetWeatherForecasts();
}
```

### 3. 注册服务

```csharp
var builder = WebApplication.CreateBuilder(args);

// 添加服务
builder.Services.AddTransient<SimpleServices>();
// 添加端点
builder.Services.AddEndpoints();

var app = builder.Build();

// 映射端点
app.MapEndpoints();

app.Run();
```

## 核心概念

### ApiEndpoints 基类

提供对 HttpContext 的便捷访问：

```csharp
public abstract class ApiEndpoints
{
    protected HttpContext Context { get; set; }
    protected HttpRequest Request => Context.Request;
    protected HttpResponse Response => Context.Response;
    // ... 其他便捷属性
}
```

### ApiEndpointsAttribute

标记类为 API 端点集合：

```csharp
[AttributeUsage(AttributeTargets.Class)]
public sealed class ApiEndpointsAttribute : Attribute
{
}
```

## AOT 兼容性

- .NET 9.0 及以上版本：完全支持 AOT
- .NET 8.0 及更早版本：不支持 AOT，因为依赖 TypeDescriptor 的类型转换

## 限制

- 不支持泛型类型
- 不支持抽象类型
- .NET 8.0 及更早版本不支持 AOT

## 许可证

[MIT License](LICENSE)