# Panda.DynamicWebApi

`Panda.DynamicWebApi` 是一个动态生成WebApi的组件，生成的API符合Restful风格，受启发于ABP。它可以根据符合条件的类来生成WebApi，由MVC框架直接调用逻辑，无性能问题，完美兼容Swagger来构建API说明文档，与自己编写Controller相比并无区别。

应用场景：DDD架构中的应用逻辑层，可使用本组件来直接生成WebApi，而无需再用Controller来调用。

## 1.入门

## 3.引用项目

> 本项目直接或间接引用了以下项目

- [ABP Vnext](https://github.com/abpframework/abp)