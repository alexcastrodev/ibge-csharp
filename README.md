# Introduction



# Requirements

- .NET 7
- Docker (optional)

# How to use

## Using without Docker

### restore

```bash
dotnet run restore
```

### build

```bash
dotnet run build
```

# run

```bash
dotnet run --project Ibge
```

### Requests

Get all locations

```bash
curl -X 'GET' \
  'https://localhost:7186/v1/Locations' \
  -H 'accept: application/json'
```



# SwaggerUI

https://localhost:7186/swagger/index.html


# References

This following links are the references i read for build this project:

https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt

https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0&viewFallbackFrom=aspnetcore-2.2#actionresultt-type

https://learn.microsoft.com/en-us/previous-versions/aspnet/cc668224(v=vs.100)

https://www.youtube.com/watch?v=kgzc_gw2pi8

https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties?pivots=dotnet-7-0