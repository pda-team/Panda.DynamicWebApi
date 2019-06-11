# Panda.DynamicWebApi

`Panda.DynamicWebApi` 是一个动态生成WebApi的组件，生成的API符合Restful风格，受启发于ABP。它可以根据符合条件的类来生成WebApi，由MVC框架直接调用逻辑，无性能问题，完美兼容Swagger来构建API说明文档，与手动编写Controller相比并无区别。

应用场景：DDD架构中的应用逻辑层，可使用本组件来直接生成WebApi，而无需再用Controller来调用。

[![Latest version](https://img.shields.io/nuget/v/Panda.DynamicWebApi.svg)](https://www.nuget.org/packages/Panda.DynamicWebApi/)

## 1.快速入门

（1）新建一个 ASP.NET Core WebApi(或MVC) 项目

（2）通过Nuget安装组件

````shell
Install-Package Panda.DynamicWebApi
````

（3）创建一个类命名为 `AppleAppService`，实现 `IDynamicWebApi` 接口，并加入特性 `[DynamicWebApi]`

````csharp
[DynamicWebApi]
public class AppleAppService: IDynamicWebApi
{
    private static readonly Dictionary<int,string> Apples=new Dictionary<int, string>()
    {
        [1]="Big Apple",
        [2]="Small Apple"
    };

    /// <summary>
    /// Get An Apple.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public string Get(int id)
    {
        if (Apples.ContainsKey(id))
        {
            return Apples[id];
        }
        else
        {
            return "No Apple!";
        }
    }

    /// <summary>
    /// Get All Apple.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> Get()
    {
        return Apples.Values;
    }

    public void Update(UpdateAppleDto dto)
    {
        if (Apples.ContainsKey(dto.Id))
        {
            Apples[dto.Id] =dto.Name;
        }
    }

    /// <summary>
    /// Delete Apple
    /// </summary>
    /// <param name="id">Apple Id</param>
    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        if (Apples.ContainsKey(id))
        {
            Apples.Remove(id);
        }
    }

}
````

（4）在 Startup 中注册 DynamicWebApi

````csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDynamicWebApi();
}
````

（5）添加 Swagger 

（6）运行

运行浏览器以后访问 `<你的项目地址>/swagger/index.html`，将会看到为我们 `AppleService` 生成的 WebApi

![1560265120580](assets/1560265120580.png)

本快速入门 Demo 地址：[点我](/samples/Panda.DynamicWebApiSample)

## 2.更进一步

（1）要让类生成动态API需要满足两个条件，一个是该类**直接**或**间接**实现 `IDynamicWebApi`，同时该类**本身**或者**父类**或者**实现的接口**具有特性 `DynamicWebApi`

（2）添加特性 `[NonDynamicWebApi]` 可使一个类或者一个方法不生成API，`[NonDynamicWebApi]` 具有最高的优先级。

（3）会对符合规则的动态API**类名**进行后缀的删除，如：我们快速入门的 `AppleAppService`，会被删除 AppService 后缀，这个规则是可以动态配置的。

（4）会自动添加API路由前缀，默认会为所有API添加 `api`前缀

（5）默认的HTTP动词为`POST`，可以理解为API的 Http Method。但可以通过 `HttpGet/HttpPost/HttpDelete `等等ASP.NET Core 内置特性来覆盖

（6）可以通过`HttpGet/HttpPost/HttpDelete `等内置特性来覆盖默认路由

（7）默认会根据你的方法名字来设置HTTP动词，如 CreateApple 或者 Create 生成的API动词为 `POST`，对照表如下，若命中（忽略大小写）对照表那么该API的名称中的这部分将会被省略，如 CreateApple 将会变成 Apple，如未在以下对照表中，将会使用默认动词 `POST`

| 方法名开头 | 动词   |
| ---------- | ------ |
| create     | POST   |
| add        | POST   |
| post       | POST   |
| get        | GET    |
| find       | GET    |
| fetch      | GET    |
| query      | GET    |
| update     | PUT    |
| put        | PUT    |
| delete     | DELETE |
| remove     | DELETE |

（8）强烈建议方法名称使用帕斯卡命名（PascalCase）规范，且使用以上对照表的动词。如:

添加苹果 -> Add/AddApple/Create/CreateApple

更新苹果 -> Update/UpdateApple

...

## 3.配置

所有的配置均在对象 `DynamicWebApiOptions` 中，说明如下：

| 属性名                      | 是否必须 | 说明                                                      |
| --------------------------- | -------- | --------------------------------------------------------- |
| DefaultHttpVerb             | 否       | 默认值：POST。默认HTTP动词                                |
| DefaultAreaName             | 否       | 默认值：空。Area 路由名称                                 |
| DefaultApiPreFix            | 否       | 默认值：api。API路由前缀                                  |
| ApiRemovePostfixes          | 否       | 默认值：AppService/ApplicationService。类名需要移除的后缀 |
| FormBodyBindingIgnoredTypes | 否       | 默认值：IFormFile。不通过MVC绑定到参数列表的类型。        |

## 4.疑难解答

若遇到问题，可使用 [Issues](https://github.com/dotnetauth/Panda.DynamicWebApi/issues) 进行提问。

## 5.引用项目说明

> 本项目直接或间接引用了以下项目

- [ABP Vnext](https://github.com/abpframework/abp)
