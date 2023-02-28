using IMG.Plugins.InMemory;
using IMS.UseCases.Actvities;
using IMS.UseCases.Actvities.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Products.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();

builder.Services.AddTransient<IViewInventoriesByNameUseCases, ViewInventoriesByNameUseCases>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();


builder.Services.AddTransient<IViewProductsByNameUseCases, ViewProductsByNameUseCases>();
builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
builder.Services.AddTransient<IViewProductsByIdUseCase, ViewProductsByIdUseCase>();
builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();

builder.Services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();


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
