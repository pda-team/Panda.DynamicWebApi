# Panda.DynamicWebApi

`Panda.DynamicWebApi` 是一个动态生成WebApi的组件，生成的API符合Restful风格，受启发于ABP。它可以根据符合条件的类来生成WebApi，由MVC框架直接调用逻辑，无性能问题，完美兼容Swagger来构建API说明文档，与自己编写Controller相比并无区别。

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
    services.AddDynamicWebApi(new DynamicWebApiOptions(){ApiAssemblies = new List<Assembly>(){ this.GetType().Assembly } });
}
````

（5）添加 Swagger 

（6）运行

运行浏览器以后访问 `<你的项目地址>/swagger/index.html`，将会看到为我们 `AppleService` 生成的 WebApi

![1560265120580](assets/1560265120580.png)

本快速入门 Demo 地址：[点我](/samples/Panda.DynamicWebApiSample)



## 3.引用项目

> 本项目直接或间接引用了以下项目

- [ABP Vnext](https://github.com/abpframework/abp)