---
title: "项目概要介绍"
author: zhangbig，郑文尧
date: April 22, 2021
output: word_document
export_on_save:
  pandoc: true
---
# 项目概要介绍

## 整体背景

排产是B端用户强需求的一种场景，通过智能排产，优化生产过程，提高人员、流程的效率，提高交付率，设备利用率等，是实际生产场景中核心的组成部分；排产涉及前期的销售，供应链，仓储，生产，客户关系等复杂系统，成功的自动化排产产品需要其他应用的数据层支持， 开发难度、系统集成度较高。当工厂的设备，人员，订单增多后普通人工排产存在局限性，多变量多约束的排产需要通过计算机来完成。

## 业务背景

工业互联网解决的一个问题是工业生产过程中的资源分配问题，排产是其中典型的问题场景；排产作为工业生产中的核心，对其进行针对性的产品研发，通过应用AI技术、运筹学等强大的算法工具，通过对资源的优化调度，能够快速的在多因素、多约束的问题背景下，找到最合理的生产方式，使资源最大化利用，提升产能，降低成本，提升公司业务水平，推进行业进步。

## 项目主要业务功能

1. 充分考虑工厂中存在的场景，将商品由若干个半成品装配而成，每个半成品有多道工序制作而成，且工序之间存在前后置关系。后台会根据这些商品进行建模，运用OR-Tools工具对工序进行编排，使得资源最大化利用，提升产能，最小化生产时间。

2. 实现web端的排产平台，web端能够对后台排产的数据运用甘特图的形式进行可视化的展示并且管理员可以在工序满足前后置的情况下对工序进行手动的调整。

3. web端同时可以实现对订单及资源信息的导入导出，同时也可以根据需要进行插单处理，让工厂的利益最大化。

## 运用的技术

### 后端技术

- 由 Asp.net core 3.1 搭建的 Web Api 服务器，为客户端提供数据以及控制的接口
- Sql Server 2019数据库
- AutoMapper对象映射
- Entity Framework Core
- swagger

### 前端技术

- Vue 2.6.x
- gantt-schedule-timeline-calendar甘特图工具
- Axios
- Element UI

### 求解器

- Google Or-Tools
