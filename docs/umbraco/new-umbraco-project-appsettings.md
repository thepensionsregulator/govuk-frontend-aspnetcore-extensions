# Recommended appsettings configuration for a new Umbraco project

1. In `appsettings.json` replace the `Umbraco` section with the following configuration. This includes the [Paste from Word plugin for TinyMCE](https://github.com/pangaeatech/tinymce-paste-from-word-plugin).

   ```json
   {
     "Umbraco": {
       "CMS": {
         "Global": {
           "Id": "<generated-id>",
           "UseHttps": true,
           "SanitizeTinyMce": true
         },
         "Content": {
           "AllowEditInvariantFromNonDefault": true,
           "ContentVersionCleanupPolicy": {
             "EnableCleanup": true
           }
         },
         "Unattended": {
           "UpgradeUnattended": true
         },
         "Runtime": {
           "Mode": "Production"
         },
         "ModelsBuilder": {
           "ModelsMode": "Nothing"
         },
         "Security": {
           "AllowConcurrentLogins": false
         },
         "RichTextEditor": {
           "ValidElements": "\u002Ba[id|rel|data-id|data-udi|rev|charset|hreflang|lang|tabindex|type|name|href|target|class],-strong/-b[class],-em/-i[class],-strike[class],p[id|style|class],-ol[style|class|reversed|start|type],-ul[style|class],-li[class],br[class],-sub[class],-sup[class],-blockquote[class],-table[class|id|lang],-tr[id|lang|class|rowspan],tbody[id|class],thead[id|class],tfoot[id|class],#td[id|lang|class|colspan|rowspan|width],#th[id|lang|class|colspan|rowspan|width|scope],caption[id|lang|class],-div[id|class],-span[class],-pre[class],-h1[id|class],-h2[id|class],-h3[id|class],-h4[id|class],-h5[id|class],-h6[id|class],hr[class],small[class],dd[id|class|lang],dl[id|class|lang],dt[id|class|dir|lang]"
         },
         "TinyMceConfig": {
           "customConfig": {
             "table_advtab": false,
             "table_cell_advtab": false,
             "table_row_advtab": false,
             "table_class_list": [
               {
                 "title": "None",
                 "value": ""
               },
               {
                 "title": "Width: three-quarters",
                 "value": "govuk-!-width-three-quarters"
               },
               {
                 "title": "Width: two-thirds",
                 "value": "govuk-!-width-two-thirds"
               },
               {
                 "title": "Width: one-half",
                 "value": "govuk-!-width-one-half"
               }
             ],
             "table_cell_class_list": [
               {
                 "title": "None",
                 "value": ""
               },
               {
                 "title": "Header cell",
                 "value": "govuk-table__header"
               },
               {
                 "title": "Numeric header cell",
                 "value": "govuk-table__header--numeric"
               },
               {
                 "title": "Numeric data cell",
                 "value": "govuk-table__cell--numeric"
               },
               {
                 "title": "Width: one-half",
                 "value": "govuk-!-width-one-half"
               },
               {
                 "title": "Width: one-third",
                 "value": "govuk-!-width-one-third"
               },
               {
                 "title": "Width: one-quarter",
                 "value": "govuk-!-width-one-quarter"
               }
             ],
             "table_header_type": "sectionCells",
             "table_sizing_mode": "relative",
             "table_resize_bars": "false",
             "object_resizing": "img",
             "external_plugins": {
               "paste_from_word": "/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/paste-from-word.min.js"
             },
             "paste_webkit_styles": "all",
             "paste_remove_styles_if_webkit": false
           }
         }
       }
     }
   }
   ```

2. In `appsettings.Development.json` replace the `Umbraco` section with the following configuration.

   ```json
   {
     "Umbraco": {
       "CMS": {
         "Global": {
           "UseHttps": true
         },
         "Content": {
           "MacroErrors": "Throw"
         },
         "Runtime": {
           "Mode": "Development"
         },
         "ModelsBuilder": {
           "ModelsMode": "SourceCodeManual",
           "ModelsDirectory": "~/Models/ModelsBuilder",
           "IncludeVersionNumberInGeneratedModels": false
         },
         "Hosting": {
           "Debug": true
         }
       }
     }
   }
   ```

3. In `appsettings.Development.json` delete the `ConnectionStrings:umbracoDbDSN` and `ConnectionStrings:umbracoDbDSN_ProviderName` settings (these will be re-instated when you run the application).
