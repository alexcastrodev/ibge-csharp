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

