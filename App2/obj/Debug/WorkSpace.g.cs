﻿

#pragma checksum "C:\Users\Peter\Documents\Visual Studio 2012\Projects\App2\App2\WorkSpace.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "29463BA230B5F55ED731C6F5AC2E27A6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntoTheBrain
{
    partial class WorkSpace : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 10 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.Page_Loaded_1;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 85 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.Controls.ToggleSwitch)(target)).Toggled += this.ToggleSwitch_Toggled_1;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 78 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.OkClick;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 53 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ItemListViewSelectionChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 45 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.TextBox_TextChanged_1;
                 #line default
                 #line hidden
                #line 45 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyDown += this.notEnterLettersKeyDown;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 26 "..\..\WorkSpace.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.backButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


