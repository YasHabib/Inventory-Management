using IMG.Plugins.EFCoreSQL;
using IMG.Plugins.InMemory;
using IMS.Plugins.EFCoreSQL;
using IMS.UseCases.Actvities;
using IMS.UseCases.Actvities.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Products.Interface;
using IMS.UseCases.Reporting;
using IMS.UseCases.Reporting.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<IMSContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagement"));
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


if (builder.Environment.IsEnvironment("TESTING"))
{
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
    //In-memory data-store
    builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
    builder.Services.AddSingleton<IProductRepository, ProductRepository>();
    builder.Services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
    builder.Services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
}
else
{
    //EFCore data-store
    builder.Services.AddTransient<IInventoryRepository, InventoryEFCoreRepository>();
    builder.Services.AddTransient<IProductRepository, ProductEFCoreRepository>();
    builder.Services.AddTransient<IInventoryTransactionRepository, InventoryTransactionEFCoreRepository>();
    builder.Services.AddTransient<IProductTransactionRepository, ProductTransactionEFCoreRepository>();

}


builder.Services.AddTransient<IViewInventoriesByNameUseCases, ViewInventoriesByNameUseCases>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();


builder.Services.AddTransient<IViewProductsByNameUseCases, ViewProductsByNameUseCases>();
builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
builder.Services.AddTransient<IViewProductsByIdUseCase, ViewProductsByIdUseCase>();
builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();

builder.Services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();
builder.Services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();
builder.Services.AddTransient<ISellProductUseCase, SellProductUseCase>();

builder.Services.AddTransient<ISearchInventoryTransactionsUseCase, SearchInventoryTransactionsUseCase>();
builder.Services.AddTransient<ISearchProductTransactionsUseCase, SearchProductTransactionsUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
