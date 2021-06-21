#pragma checksum "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "32908057431adf065d5947bd3a850726e7aab1cb"
// <auto-generated/>
#pragma warning disable 1591
namespace ServerProjectTracker.Pages.Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    public partial class ProjectComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "card");
            __builder.OpenElement(2, "img");
            __builder.AddAttribute(3, "class", "card-img-top");
            __builder.AddAttribute(4, "src", 
#nullable restore
#line 2 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                                    ImageLink

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(5, "alt", 
#nullable restore
#line 2 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                                                     Title

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(6, "\r\n    ");
            __builder.OpenElement(7, "div");
            __builder.AddAttribute(8, "class", "card-body");
            __builder.OpenElement(9, "div");
            __builder.AddAttribute(10, "class", "d-flex justify-content-between");
            __builder.OpenElement(11, "h5");
            __builder.AddContent(12, 
#nullable restore
#line 5 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                 Title

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n            ");
            __builder.OpenElement(14, "h6");
            __builder.AddContent(15, 
#nullable restore
#line 6 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                 Language

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(16, "\r\n        ");
            __builder.OpenElement(17, "p");
            __builder.AddAttribute(18, "class", "card-text");
            __builder.AddContent(19, 
#nullable restore
#line 8 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                              Description

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(20, "\r\n        ");
            __builder.OpenElement(21, "div");
            __builder.AddAttribute(22, "class", "d-flex justify-content-between");
            __builder.AddAttribute(23, "style", "margin-bottom: 1em");
            __builder.OpenElement(24, "div");
            __builder.AddAttribute(25, "class");
            __builder.AddMarkupContent(26, "\r\n                Added At: ");
            __builder.AddContent(27, 
#nullable restore
#line 11 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                           AddTime.ToString("MM/dd/yyyy")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(28, "\r\n            ");
            __builder.OpenElement(29, "div");
            __builder.AddAttribute(30, "class");
            __builder.AddMarkupContent(31, "\r\n                Added At: ");
            __builder.AddContent(32, 
#nullable restore
#line 14 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                           UpdateTime.ToString("MM/dd/yyyy")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(33, "\r\n        ");
            __builder.OpenElement(34, "div");
            __builder.AddAttribute(35, "class", "d-flex justify-content-between");
            __builder.OpenElement(36, "div");
            __builder.AddAttribute(37, "class");
            __builder.OpenElement(38, "a");
            __builder.AddAttribute(39, "href", 
#nullable restore
#line 19 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                          ProjectDetailsLink

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(40, "class", "btn btn-primary");
            __builder.AddContent(41, "View Details");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(42, "\r\n            ");
            __builder.OpenElement(43, "div");
            __builder.AddAttribute(44, "class");
            __builder.OpenElement(45, "a");
            __builder.AddAttribute(46, "href", 
#nullable restore
#line 22 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
                          ProjectLink

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(47, "class", "btn btn-primary");
            __builder.AddContent(48, "Go To Project");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 28 "C:\Users\luke\Documents\GitHub\Server-Manager\ServerProjectTracker\ServerProjectTracker\Pages\Components\ProjectComponent.razor"
       
    [Parameter]
    public string Title { get; set; }
    [Parameter]
    public string Description { get; set; }
    [Parameter]
    public string Language { get; set; }
    [Parameter]
    public DateTime AddTime { get; set; }
    [Parameter]
    public DateTime UpdateTime { get; set; }
    [Parameter]
    public string ImageLink { get; set; }
    [Parameter]
    public string ProjectLink { get; set; }
    [Parameter]
    public string ProjectDetailsLink { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
