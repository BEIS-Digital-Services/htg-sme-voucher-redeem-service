FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VoucherRedeemMicroService/VoucherRedeemMicroService.csproj","VoucherRedeemMicroService/"]
RUN dotnet restore "VoucherRedeemMicroService/VoucherRedeemMicroService.csproj"
COPY . .
WORKDIR "/src/VoucherRedeemMicroService"
RUN dotnet publish "VoucherRedeemMicroService.csproj" -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "VoucherRedeemMicroService.dll"]