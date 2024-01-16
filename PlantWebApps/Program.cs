using PlantWebApps.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(options => 
{
    options.Filters.Add<GetIdentity>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "JobDispatchEditReport",
    pattern: "JobDispatch/Report/{id}",
    defaults: new { controller = "JobDispatch", action = "Report" }
);

app.MapControllerRoute(
    name: "JobDispatchSearchWonoDetail",
    pattern: "JobDispatch/searchwonodetail/{id}/{wono}",
    defaults: new { controller = "JobDispatch", action = "SearchWonoDetail" }
);

app.MapControllerRoute(
	name: "ExrRepairJobHistoryForm",
	pattern: "ExrRepairJobHistory/Edit/{id}",
	defaults: new { controller = "ExrRepairJobHistory", action = "Edit" }
);

app.MapControllerRoute(
    name: "ExrRepairJobHistoryInvestigationOldReportPic",
    pattern: "ExrRepairJobHistoryInspection/OldReportPictBody/{wo}",
    defaults: new { controller = "ExrRepairJobHistoryInspection", action = "OldReportPictBody" }
);

app.MapControllerRoute(
	name: "ExrRepairJobHistoryInvestigationOldReportPicHeader",
	pattern: "ExrRepairJobHistoryInspection/OldReportPictHeader/{wo}",
	defaults: new { controller = "ExrRepairJobHistoryInspection", action = "OldReportPictHeader" }
);

app.Run();
