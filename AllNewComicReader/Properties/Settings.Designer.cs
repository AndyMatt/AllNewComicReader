﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AllNewComicReader.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int MainMenuOffset {
            get {
                return ((int)(this["MainMenuOffset"]));
            }
            set {
                this["MainMenuOffset"] = value;
            }
        }
               
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MainMenuEnabled {
            get {
                return ((bool)(this["MainMenuEnabled"]));
            }
            set {
                this["MainMenuEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Width")]
        public global::AllNewComicReader.ViewSetting PageView {
            get {
                return ((global::AllNewComicReader.ViewSetting)(this["PageView"]));
            }
            set {
                this["PageView"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Lanczos")]
        public global::ImageMagick.FilterType ImageFilter {
            get {
                return ((global::ImageMagick.FilterType)(this["ImageFilter"]));
            }
            set {
                this["ImageFilter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DoublePage {
            get {
                return ((bool)(this["DoublePage"]));
            }
            set {
                this["DoublePage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SupressForSpread {
            get {
                return ((bool)(this["SupressForSpread"]));
            }
            set {
                this["SupressForSpread"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TTPageNumber {
            get {
                return ((bool)(this["TTPageNumber"]));
            }
            set {
                this["TTPageNumber"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TTPageInfo {
            get {
                return ((bool)(this["TTPageInfo"]));
            }
            set {
                this["TTPageInfo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool HideCursor {
            get {
                return ((bool)(this["HideCursor"]));
            }
            set {
                this["HideCursor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoContrast {
            get {
                return ((bool)(this["AutoContrast"]));
            }
            set {
                this["AutoContrast"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoYellowBalance {
            get {
                return ((bool)(this["AutoYellowBalance"]));
            }
            set {
                this["AutoYellowBalance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoLevel {
            get {
                return ((bool)(this["AutoLevel"]));
            }
            set {
                this["AutoLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public float Brightness {
            get {
                return ((float)(this["Brightness"]));
            }
            set {
                this["Brightness"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public float Hue {
            get {
                return ((float)(this["Hue"]));
            }
            set {
                this["Hue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PageUp")]
        public string InputPrevPage {
            get {
                return ((string)(this["InputPrevPage"]));
            }
            set {
                this["InputPrevPage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("End")]
        public string InputLastPage {
            get {
                return ((string)(this["InputLastPage"]));
            }
            set {
                this["InputLastPage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Home")]
        public string InputFirstPage {
            get {
                return ((string)(this["InputFirstPage"]));
            }
            set {
                this["InputFirstPage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Up")]
        public string InputScrollUp {
            get {
                return ((string)(this["InputScrollUp"]));
            }
            set {
                this["InputScrollUp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Down")]
        public string InputScrollDown {
            get {
                return ((string)(this["InputScrollDown"]));
            }
            set {
                this["InputScrollDown"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Left")]
        public string InputScrollLeft {
            get {
                return ((string)(this["InputScrollLeft"]));
            }
            set {
                this["InputScrollLeft"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Right")]
        public string InputScrollRight {
            get {
                return ((string)(this["InputScrollRight"]));
            }
            set {
                this["InputScrollRight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("W")]
        public string InputFullscreen {
            get {
                return ((string)(this["InputFullscreen"]));
            }
            set {
                this["InputFullscreen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("T")]
        public string InputToolbar {
            get {
                return ((string)(this["InputToolbar"]));
            }
            set {
                this["InputToolbar"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("V")]
        public string InputViewMode {
            get {
                return ((string)(this["InputViewMode"]));
            }
            set {
                this["InputViewMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("P")]
        public string InputDoublePage {
            get {
                return ((string)(this["InputDoublePage"]));
            }
            set {
                this["InputDoublePage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("N")]
        public string InputPageNumber {
            get {
                return ((string)(this["InputPageNumber"]));
            }
            set {
                this["InputPageNumber"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("I")]
        public string InputPageInfo {
            get {
                return ((string)(this["InputPageInfo"]));
            }
            set {
                this["InputPageInfo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("R")]
        public string InputRotate {
            get {
                return ((string)(this["InputRotate"]));
            }
            set {
                this["InputRotate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ctrl+Right")]
        public string InputLoadNext {
            get {
                return ((string)(this["InputLoadNext"]));
            }
            set {
                this["InputLoadNext"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ctrl+Left")]
        public string InputLoadPrev {
            get {
                return ((string)(this["InputLoadPrev"]));
            }
            set {
                this["InputLoadPrev"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Escape")]
        public string InputExit {
            get {
                return ((string)(this["InputExit"]));
            }
            set {
                this["InputExit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse1Double {
            get {
                return ((string)(this["Mouse1Double"]));
            }
            set {
                this["Mouse1Double"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse2Double {
            get {
                return ((string)(this["Mouse2Double"]));
            }
            set {
                this["Mouse2Double"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse3Double {
            get {
                return ((string)(this["Mouse3Double"]));
            }
            set {
                this["Mouse3Double"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse4Double {
            get {
                return ((string)(this["Mouse4Double"]));
            }
            set {
                this["Mouse4Double"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse5Double {
            get {
                return ((string)(this["Mouse5Double"]));
            }
            set {
                this["Mouse5Double"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse1Click {
            get {
                return ((string)(this["Mouse1Click"]));
            }
            set {
                this["Mouse1Click"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse2Click {
            get {
                return ((string)(this["Mouse2Click"]));
            }
            set {
                this["Mouse2Click"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ContextMenu")]
        public string Mouse3Click {
            get {
                return ((string)(this["Mouse3Click"]));
            }
            set {
                this["Mouse3Click"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PreviousPage")]
        public string Mouse4Click {
            get {
                return ((string)(this["Mouse4Click"]));
            }
            set {
                this["Mouse4Click"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NextPage")]
        public string Mouse5Click {
            get {
                return ((string)(this["Mouse5Click"]));
            }
            set {
                this["Mouse5Click"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PageDown")]
        public string InputNextPage {
            get {
                return ((string)(this["InputNextPage"]));
            }
            set {
                this["InputNextPage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public float ScrollAmount {
            get {
                return ((float)(this["ScrollAmount"]));
            }
            set {
                this["ScrollAmount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Average9")]
        public global::ImageMagick.PixelInterpolateMethod PixelInterpolation {
            get {
                return ((global::ImageMagick.PixelInterpolateMethod)(this["PixelInterpolation"]));
            }
            set {
                this["PixelInterpolation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Y")]
        public string InputYellow {
            get {
                return ((string)(this["InputYellow"]));
            }
            set {
                this["InputYellow"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Bookmarks {
            get {
                return ((bool)(this["Bookmarks"]));
            }
            set {
                this["Bookmarks"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool BookmarkAsk {
            get {
                return ((bool)(this["BookmarkAsk"]));
            }
            set {
                this["BookmarkAsk"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool HueSaturation {
            get {
                return ((bool)(this["HueSaturation"]));
            }
            set {
                this["HueSaturation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public float Saturation {
            get {
                return ((float)(this["Saturation"]));
            }
            set {
                this["Saturation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MoveImage")]
        public string Mouse1Hold {
            get {
                return ((string)(this["Mouse1Hold"]));
            }
            set {
                this["Mouse1Hold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MoveImage")]
        public string Mouse2Hold {
            get {
                return ((string)(this["Mouse2Hold"]));
            }
            set {
                this["Mouse2Hold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse3Hold {
            get {
                return ((string)(this["Mouse3Hold"]));
            }
            set {
                this["Mouse3Hold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse4Hold {
            get {
                return ((string)(this["Mouse4Hold"]));
            }
            set {
                this["Mouse4Hold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Mouse5Hold {
            get {
                return ((string)(this["Mouse5Hold"]));
            }
            set {
                this["Mouse5Hold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PageContolPopup {
            get {
                return ((bool)(this["PageContolPopup"]));
            }
            set {
                this["PageContolPopup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Fullscreen {
            get {
                return ((bool)(this["Fullscreen"]));
            }
            set {
                this["Fullscreen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FastRender {
            get {
                return ((bool)(this["FastRender"]));
            }
            set {
                this["FastRender"] = value;
            }
        }
    }
}
