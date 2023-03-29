# 变更日志

## v2.1.2

+ 修复`SkiaSharp`在`Linux`环境下异常

## v2.1.1

+ 修复`System.Drawing.Common`在`Linux`环境下异常

## v2.1.0

+ 升级`Sixpence.ORM 3.1.0`

## v2.0.0

+ 升级 .net 6.0

## v1.5.3

+ 新增系统参数：Github、Gitee
+ EntityBaseController 方法修改为虚方法

## v1.5.2

+ 系统参数添加描述字段
+ 添加 SysConfigCache，移除 SysConfigFactory

## v1.5.1

+ 修复菜单获取失败
+ 修改模板目录名

## v1.5.0

+ 实体命名改为 Pascal 规则

## v1.4.0

+ 修复了系统创建用户失败

## v1.3.0

+ version 不再发布时编译

## v1.2.0

+ 优化了启动类

## v1.1.0

+ 第三方登录策略类添加策略名
+ 移除了Swagger启动
+ 修复了脚本版本错误

## v1.0.0

一个基于`.net core 6`的`WebApi`框架，通过此框架能迅速搭建一个接口平台

### 功能

+ 基础功能
  + 登录/注册
    + JWT
    + 集成了Github、Gitee等第三方登录策略
    + 支持邮箱注册
  + 基于RBAC的角色管理
    + 权限管理
    + 角色管理
    + 用户管理
  + 图库
    + 本地图片
    + Pixabay 云图库
  + 作业管理（quartz）
  + 数据库表维护
    + 实体管理
    + 字段管理
  + 选项集
  + 版本管理
    + 支持CSV文件、SQL脚本初始化数据
    + 启动自动执行迁移脚本
+ 集成 Swagger（手动开启）

