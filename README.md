# V2Sharp

> V2Ray Trafficview panel WebAPI server (Unfinished)

## Introduction

To check the traffic usage of each V2Ray user, I developed a trafficview panel with Python-Flask last year. This time, I'd like to make it a more formal server, so I use C# to restructure it.

In the new Trafficview panel, I aim to separate the front-ending and back-ending completely. In other words, I don't like to use any back-ending template engine to render views.

## TODO

1. Complete `RefreshTraffic` function
2. Add JWT-Auth to check API call permission

## Technology

Language: C#

Runtime: dotNet 5.0

ORM: EntityFrameworkCore

Database: MySQL

## WebAPI designed

1. http(s)://IP:Port/api/traffic

   show traffic usage of all users

2. http(s)://IP:Port/api/traffic/{GUID}

   show traffic usage of specific user

3. http(s)://IP:Port/api/config/{GUID}

   show vmess connection config of specific user

4. http(s)://IP:Port/api/sysctl/v2restart

   restart V2Ray service

5. http(s)://IP:Port/api/sysctl/newPeriod

   start a new traffic statistic period, clear traffic log of last period

6. http(s)://IP:Port/api/sysctl/up/{GUID}

   upgrade a specific user to be Admin

7. http(s)://IP:Port/api/sysctl/down/{GUID}

   downgrade a specific user to be a normal user

