[1mdiff --cc GovUk.Frontend.AspNetCore.Extensions/wwwroot/govuk/govuk-validation.js[m
[1mindex ee54a86,9dca9f3..0000000[m
[1m--- a/GovUk.Frontend.AspNetCore.Extensions/wwwroot/govuk/govuk-validation.js[m
[1m+++ b/GovUk.Frontend.AspNetCore.Extensions/wwwroot/govuk/govuk-validation.js[m
[36m@@@ -196,8 -196,8 +196,8 @@@[m [mfunction createGovUkValidator() [m
          // Otherwise target the original element.[m
          let list = closest([m
            element,[m
[31m -          ".govuk-radios, .govuk-radios__conditional, .govuk-checkboxes, .govuk-checkboxes__conditional, .govuk-date-input"[m
[32m +          ".govuk-radios, .govuk-radios__conditional, .govuk-checkboxes, .govuk-checkboxes__conditional, .govuk-date-input, .govuk-input__wrapper"[m
[31m-           );[m
[32m+         );[m
          let targetElement =[m
            list &&[m
            !list.classList.contains("govuk-radios__conditional") &&[m
[36m@@@ -261,74 -261,74 +261,78 @@@[m
      },[m
  [m
      validateElement: function (element) {[m
[31m--        const validator = govuk.getValidator(element.form);[m
[32m++      const validator = govuk.getValidator(element.form);[m
[32m++[m
[32m++      let data = {};[m
[32m++      [].forEach.call(element.attributes, function (attr) {[m
[32m++        // find all data-val attributes for this element[m
[32m++        if (/^data-val/.test(attr.name)) {[m
[32m++          var ruleName = attr.name.substr(9); // Creates a dictionary - keys look like data-val-[KEY][m
[32m++          data[ruleName] = attr.value;[m
[32m++        }[m
[32m++      });[m
  [m
[31m--        let data = {};[m
[31m--        [].forEach.call(element.attributes, function (attr) { // find all data-val attributes for this element[m
[31m--            if (/^data-val/.test(attr.name)) {[m
[31m--                var ruleName = attr.name.substr(9); // Creates a dictionary - keys look like data-val-[KEY][m
[31m--                data[ruleName] = attr.value;[m
[32m++      for (const [key, value] of Object.entries(data)) {[m
[32m++        // Rules look like data-val-[RULE_NAME] so let's find those[m
[32m++        if (!key.includes("-")) {[m
[32m++          // No hyphen means this is a rule[m
[32m++          let props = {}; // Find properties associated with rule[m
[32m++          for (const [k, v] of Object.entries(data)) {[m
[32m++            if (k.startsWith(key + "-")) {[m
[32m++              // look for attributes that look like [RULE_NAME]-property - e.g. range-max[m
[32m++              let prop = k.replace(key + "-", "");[m
[32m++              props[prop] = v;[m
[32m++            }[m
[32m++          }[m
[32m++[m
[32m++          if (key) {[m
[32m++            let method = key;[m
[32m++            // conditional switch on anything that doesn't match - data-val-length[m
[32m++            if (method == "length" && props) {[m
[32m++              // Now query props to see which rule we have to call...[m
[32m++              let propsKeys = Object.keys(props);[m
[32m++              let hasMin = propsKeys.find((e) => e == "min");[m
[32m++              let hasMax = propsKeys.find((e) => e == "max");[m
[32m++[m
[32m++              if (hasMax && hasMin) {[m
[32m++                method = "rangelength";[m
[32m++              }[m
[32m++            }[m
[32m++[m
[32m++            if (method == "minlength" && props) {[m
[32m++              let min = parseInt(props["min"]);[m
[32m++              props = min;[m
[32m +            }[m
[31m-         });[m
[32m +[m
[31m-         for (const [key, value] of Object.entries(data)) {      // Rules look like data-val-[RULE_NAME] so let's find those[m
[31m-             if (!key.includes("-")) {                           // No hyphen means this is a rule                [m
[31m-                 let props = {};                                 // Find properties associated with rule[m
[31m-                 for (const [k, v] of Object.entries(data)) {[m
[31m-                     if (k.startsWith(key + "-")) {              // look for attributes that look like [RULE_NAME]-property - e.g. range-max[m
[31m-                         let prop = k.replace(key + "-", "");[m
[31m-                         props[prop] = v;[m
[31m-                     }[m
[31m-                 }[m
[31m- [m
[31m-                 if (key) {[m
[31m-                     let method = key;[m
[31m-                     // conditional switch on anything that doesn't match - data-val-length[m
[31m-                     if (method == "length" && props) {[m
[31m-                         // Now query props to see which rule we have to call...[m
[31m-                         let propsKeys = Object.keys(props);[m
[31m-                         let hasMin = propsKeys.find(e => e == "min");[m
[31m-                         let hasMax = propsKeys.find(e => e == "max");[m
[31m- [m
[31m-                         if (hasMax && hasMin) {[m
[31m-                             method = "rangelength";[m
[31m-                         }[m
[31m-                     }[m
[31m- [m
[31m-                     if (method == "minlength" && props) {[m
[31m-                         let min = parseInt(props["min"]);[m
[31m-                         props = min;[m
[31m-                     }[m
[31m- [m
[31m-                     if (method == "maxlength" && props) {[m
[31m-                         let max = parseInt(props["max"]);[m
[31m-                         props = max;[m
[31m-                     }[m
[31m- [m
[31m-                     if (method == "equalto") {[m
[31m-                         method = "equalTo";[m
[31m-                         let other = props["other"];[m
[31m-                         props = document.getElementById(other);[m
[31m-                     }[m
[31m- [m
[31m-                     if (method == "regex") {[m
[31m-                         let pattern = props["pattern"];[m
[31m-                         props = pattern;[m
[31m-                     }[m
[31m- [m
[31m-                     let valid = validator.methods[method].call([m
[31m-                         validator.prototype,[m
[31m-                         element.value,[m
[31m-                         element,[m
[31m-                         props[m
[31m-                     );[m
[31m-                     if (!valid) {[m
[31m-                         govuk.updateError(element, value);[m
[31m-                         break;  // Break so that the errors are displayed in the order they're specified[m
[31m-                     }[m
[31m-                 }[m
[32m++            if (method == "maxlength" && props) {[m
[32m++              let max = parseInt(props["max"]);[m
[32m++              props = max;[m
[32m +            }[m
[32m++[m
[32m++            if (method == "equalto") {[m
[32m++              method = "equalTo";[m
[32m++              let other = props["other"];[m
[32m++              props = document.getElementById(other);[m
[32m++            }[m
[32m++[m
[32m++            if (method == "regex") {[m
[32m++              let pattern = props["pattern"];[m
[32m++              props = pattern;[m
[32m+             }[m
[31m -        });[m
[32m+ [m
[31m -        for (const [key, value] of Object.entries(data)) {      // Rules look like data-val-[RULE_NAME] so let's find those[m
[31m -            if (!key.includes("-")) {                           // No hyphen means this is a rule                [m
[31m -                let props = {};                                 // Find properties associated with rule[m
[31m -                for (const [k, v] of Object.entries(data)) {[m
[31m -                    if (k.startsWith(key + "-")) {              // look for attributes that look like [RULE_NAME]-property - e.g. range-max[m
[31m -                        let prop = k.replace(key + "-", "");[m
[31m -                        props[prop] = v;[m
[31m -                    }[m
[31m -                }[m
[31m -[m
[31m -                if (key) {[m
[31m -                    let method = key;[m
[31m -                    // conditional switch on anything that doesn't match - data-val-length[m
[31m -                    if (method == "length" && props) {[m
[31m -                        // Now query props to see which rule we have to call...[m
[31m -                        let propsKeys = Object.keys(props);[m
[31m -                        let hasMin = propsKeys.find(e => e == "min");[m
[31m -                        let hasMax = propsKeys.find(e => e == "max");[m
[31m -[m
[31m -                        if (hasMax && hasMin) {[m
[31m -                            method = "rangelength";[m
[31m -                        }[m
[31m -                    }[m
[31m -[m
[31m -                    if (method == "minlength" && props) {[m
[31m -                        let min = parseInt(props["min"]);[m
[31m -                        props = min;[m
[31m -                    }[m
[31m -[m
[31m -                    if (method == "maxlength" && props) {[m
[31m -                        let max = parseInt(props["max"]);[m
[31m -                        props = max;[m
[31m -                    }[m
[31m -[m
[31m -                    if (method == "equalto") {[m
[31m -                        method = "equalTo";[m
[31m -                        let other = props["other"];[m
[31m -                        props = document.getElementById(other);[m
[31m -                    }[m
[31m -[m
[31m -                    if (method == "regex") {[m
[31m -                        let pattern = props["pattern"];[m
[31m -                        props = pattern;[m
[31m -                    }[m
[31m -[m
[31m -                    let valid = validator.methods[method].call([m
[31m -                        validator.prototype,[m
[31m -                        element.value,[m
[31m -                        element,[m
[31m -                        props[m
[31m -                    );[m
[31m -                    if (!valid) {[m
[31m -                        govuk.updateError(element, value);[m
[31m -                        break;  // Break so that the errors are displayed in the order they're specified[m
[31m -                    }[m
[31m -                }[m
[32m++            let valid = validator.methods[method].call([m
[32m++              validator.prototype,[m
[32m++              element.value,[m
[32m++              element,[m
[32m++              props[m
[32m++            );[m
[32m++            if (!valid) {[m
[32m++              govuk.updateError(element, value);[m
[32m++              break; // Break so that the errors are displayed in the order they're specified[m
[32m+             }[m
[32m++          }[m
          }[m
[32m++      }[m
      },[m
  [m
      /**[m
[36m@@@ -478,15 -478,15 +482,15 @@@[m
        }[m
  [m
        return true;[m
[31m--      },[m
[32m++    },[m
  [m
      // Custom range validator to handle numbers with commas in[m
      validateRangeWithCommas(value, element, param) {[m
[31m-       var commaFreeVal = Number(value.replaceAll(",", ""));[m
[31m -      var commaFreeVal = Number(value.replace(/,/g, ''));[m
[31m -      return ([m
[31m -        this.optional(element) ||[m
[31m -        (commaFreeVal >= param[0] && commaFreeVal <= param[1])[m
[31m -      );[m
[32m++      var commaFreeVal = Number(value.replace(/,/g, ""));[m
[32m +      if (!value) {[m
[32m +        return true;[m
[32m +      }[m
[31m-       return  (commaFreeVal >= param[0] && commaFreeVal <= param[1]);[m
[32m++      return commaFreeVal >= param[0] && commaFreeVal <= param[1];[m
      },[m
    };[m
  [m
[36m@@@ -506,7 -506,7 +510,7 @@@[m [mwindow.addEventListener("DOMContentLoad[m
  [m
      validator.addMethod("phone", govuk.validatePhone);[m
      validator.addMethod("range", govuk.validateRangeWithCommas);[m
[31m--      [m
[32m++[m
      validator.unobtrusive.adapters.addBool("phone");[m
      validator.unobtrusive.parse();[m
    }[m
