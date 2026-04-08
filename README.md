# brew-coffee

A small ASP.NET Core Web API that simulates an internet-connected coffee machine.

## Endpoint

`GET /brew-coffee`

Behavior:

- Returns `200 OK` with a JSON response when coffee is available.
- Returns `503 Service Unavailable` on every fifth request.
- Returns `418 I'm a teapot` on April 1, with an empty response body.

Example success response:

```json
{
  "message": "Your piping hot coffee is ready",
  "prepared": "2021-02-03T11:56:24+0900"
}
```

## Run

```powershell
dotnet run
```

The API will start using the default ASP.NET Core launch settings.

## Test

```powershell
dotnet test brew-coffee.sln
```
