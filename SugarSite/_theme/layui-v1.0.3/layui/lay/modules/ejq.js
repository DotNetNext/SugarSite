layui.define("jquery", function (exports) { //提示：组件也可以依赖其它组件，如：layui.define('layer', callback);
    var jQuery=layui.jquery;
   (function (window, jQuery, undefined) {
       var $ = jQuery;
       jQuery.extend({
           /*随机值*/
           random: {
               //获取0-maxNum之间的随机数字
               getNum: function (maxNum) {
                   return $.random.getNumBetween(0, maxNum);
               },
               //获取min-max之间的随机数字
               getNumBetween: function (min, max) {
                   max = max + 1;
                   return Math.floor(Math.random() * (max - min)) + min;
               },
               //获取长度为length的随机数字
               getNumByLength: function (length) {
                   var array = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
                   var reval = "";
                   for (var i = 0; i < length; i++) {
                       reval += array[$.random.getNumBetween(0, array.length - 1)];
                   }
                   return reval
               },
               //获取长度为minLength-maxLength之间的随机数
               getNumBetweenLength: function (minLength, maxLength) {
                   var length = $.random.getNumBetween(minLength, maxLength);
                   return $.random.getNumByLength(length);
               },
               //获取长度wordLength（数字、字母）组成的字符串
               getWord: function (wordLength) {
                   var array = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
                   var reval = "";
                   for (var i = 0; i < wordLength; i++) {
                       reval += array[$.random.getNumBetween(0, array.length - 1)];
                   }
                   return reval;
               },
               //获取长度为minLength-maxLength之间的随机（数字、字母）组成的字符串
               getWordBetweenLength: function (minLength, maxLength) {
                   var length = $.random.getNumBetween(minLength, maxLength);
                   return $.random.getWord(length);
               },
               getGuid: function () {
                   return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                       var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                       return v.toString(16);
                   });
               }
           },

           /*linq*/
           linq: {
               contains: function (thisVal, cobj) {
                   if (jQuery.valiData.isEmpty(thisVal)) {
                       return false;
                   }
                   return thisVal.toString().lastIndexOf(cobj.toString()) != -1;
               },
               /*where*/
               where: function (obj, action) {
                   if (action == null) return;
                   var reval = new Array();
                   $(obj).each(function (i, v) {
                       if (action(v)) {
                           reval.push(v);
                       }
                   })
                   return reval;
               },
               single: function (obj, action) {
                   if (action == null) return;
                   var reval = null;
                   $(obj).each(function (i, v) {
                       if (action(v)) {
                           reval = (v);
                       }
                   })
                   return reval;
               },
               remove: function (obj, action) {
                   var removeItem = $.linq.single(obj, action);
                   obj.splice(jQuery.inArray(removeItem, obj), 1);
               },
               /*any*/
               any: function (obj, action) {
                   if (action == null) return;
                   var reval = false;
                   $(obj).each(function (i, v) {
                       if (action(v)) {
                           reval = true;
                           return false;
                       }
                   })
                   return reval;
               },
               /*select*/
               select: function (obj, action) {
                   if (action == null) return;
                   var reval = new Array();
                   $(obj).each(function (i, v) {
                       reval.push(action(v));
                   });
                   return reval;
               },
               /*each*/
               each: function (obj, action) {
                   if (action == null) return;
                   jQuery(obj).each(function (i, v) {
                       action(i, v);
                   });
               },
               /*first*/
               first: function (obj, action) {
                   if (action == null) return;
                   var reval = new Array();
                   $(obj).each(function (i, v) {
                       if (action(v)) {
                           reval.push(v);
                           return false;
                       }
                   })
                   return reval[0];
               },
               order: function (obj, field, orderByType) {
                   var p = obj;
                   p.sort(function down(x, y) {
                       if (orderByType != null && orderByType.toLocaleLowerCase() == "desc") {
                           return (x[field] < y[field]) ? 1 : -1;
                       } else {
                           return (x[field] > y[field]) ? 1 : -1;
                       }

                   })
                   return p;
               }

           },

           /*操作*/
           action: {
               url: function (actionName, hid, controllerName, areaName) {
                   if (hid == null) {
                       hid = "HidUrlAction";
                   }
                   var isAction = controllerName == null && areaName == null;
                   var isControllerName = areaName == null && controllerName != null;
                   var isArea = areaName != null;
                   var hidValue = $("#" + hid).val();
                   var regValue = hidValue.match("(^.*)/(.+)/(.+)/$");
                   var virtualDirectory = "";
                   if (regValue != null) {
                       virtualDirectory=regValue[1];
                   }
                   if (isAction) {
                       return hidValue + actionName;
                   } else if (isControllerName) {
                       areaName = regValue[2]
                       return (virtualDirectory + "/" + areaName + "/" + controllerName + "/" + actionName);
                   } else if (isArea) {
                       return (virtualDirectory + "/" + areaName + "/" + controllerName + "/" + actionName);
                   }
               },

               //移除最后一个字符
               trimEnd: function (str, c) {
                   var reg = new RegExp(c + "([^" + c + "]*?)$");
                   return str.replace(reg, function (w) { if (w.length > 1) { return w.substring(1); } else { return ""; } });
               },
               htmlEncode: function (str) {
                   return str.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
               },
               htmlDecode: function (str) {
                   return str.replace(/&amp;/g, '&').replace(/&quot;/g, '\"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
               },
               textEncode: function (str) {
                   str = str.replace(/&amp;/gi, '&');
                   str = str.replace(/</g, '&lt;');
                   str = str.replace(/>/g, '&gt;');
                   return str;
               },
               textDecode: function (str) {
                   str = str.replace(/&amp;/gi, '&');
                   str = str.replace(/&lt;/gi, '<');
                   str = str.replace(/&gt;/gi, '>');
                   return str;
               },
               //获取json的key和value
               jsonDictionary: function (json, key) {
                   var reval = new Array();
                   for (key in json) {
                       reval.push({ key: key, value: json[key] });
                   }
                   return reval;
               },
               insertStr: function (str1, n, str2) {
                   if (str1.length < n) {
                       return str1 + str2;
                   } else {
                       s1 = str1.substring(0, n);
                       s2 = str1.substring(n, str1.length);
                       return s1 + str2 + s2;
                   }
               },
               //替换所有字符
               replaceAll: function (str, findStr, reStr) {
                   var reg = new RegExp(findStr, "g");
                   return str.replace(reg, reStr);

               },
               setTimeoutWidthNum: function (fun, time, number) {
                   if (fun != null) {
                       setTimeout(function () {
                           fun();
                           number--;
                           if (number > 0) {
                               $.action.setTimeoutWidthNum(fun, time, number);
                           }
                       }, time)
                   }
               },
               //循环执行检测元素值是否有值当有值执行回调函数,超时停止检测
               elementValueReady: function (selector, fun, time) {
                   var selObj = $(selector);
                   if (time == null) time = 0;
                   time = time + 50;
                   if (time > 5000) {//间隔超过5秒则停止检测
                       return;
                   }
                   setTimeout(function () {
                       var val = "";
                       try {
                           val = selObj.val();
                       } catch (e) {

                       }
                       var valIsNull = val == null || val == "";
                       if (valIsNull) {
                           $.action.elementValueReady(selector, fun, time);
                       } else {
                           fun();
                       }
                   }, time);

               },
               //循环执行检测选择器的元素如果存在该元素，则执行回调函数过，超过停止检测
               elementNullComplate: function (selector, fun, time) {
                   var selObj = $(selector)
                   if (time == null) time = 0;
                   time = time + 50;
                   if (time > 5000) {//间隔超过5秒则停止检测
                       return;
                   }
                   setTimeout(function () {
                       if (selObj.size() > 0) {
                           $.action.elementNullComplate(selector, fun, time);
                       } else {
                           fun();
                       }
                   }, time);

               },
               //第一次执行和非第一次执行
               firstAndNotFirstMethod: function (firstExcuteMethod, noFirstExcuteMethod, key) {
                   var defaultKey = "firstAndNotFirstMethod_defaultKey";
                   if ($.valiData.isEmpty(key)) {
                       key = defaultKey;
                   }
                   var initValue = $("html").data(key);
                   if (initValue == null) {
                       firstExcuteMethod();
                       $("html").data(key, key);

                   } else {
                       noFirstExcuteMethod();
                   }
               }

           },

           /*日期时间处理*/
           getdate: {
               //获取当前日期
               getPresentDate: function () {
                   var mydate = new Date();
                   var str = "" + mydate.getFullYear() + "-";
                   str += (mydate.getMonth() + 1) + "-";
                   str += mydate.getDate();
                   return str;
               },
               //获取当前日期之前的年月date为日期"2016-6",number>0&<11月数 -leo
               //例:getfirsthalf("2016-06",5) 返回半年内的年月数组
               getfirsthalf: function (date, number) {
                   var d = new Date(date.replace(/[^\d]/g, "/") + "/1");
                   var result = [date];
                   for (var i = 0; i < number; i++) {
                       d.setMonth(d.getMonth() - 1);
                       var m = d.getMonth() + 1;
                       m = m < 10 ? "0" + m : m;
                       result.push(d.getFullYear() + "-" + m);
                   }
                   return result;
               },
               //获取一个月天数 or 最后一天  tpye=Day返回天数  否则返回最后一天日期-leo
               getLastDay: function (year, month, dateTpye) {
                   debugger
                   var new_year = year;    //取当前的年份
                   var new_month = month++;//取下一个月的第一天，方便计算（最后一天不固定）  
                   if (month > 12)            //如果当前大于12月，则年份转到下一年           
                   {
                       new_month -= 12;        //月份减            
                       new_year++;            //年份增           
                   }
                   var new_date = new Date(new_year, new_month + 1, 1);                //取当年当月中的第一天           
                   var date_count = (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();//获取当月的天数       
                   var last_date = new Date(new_date.getTime() - 1000 * 60 * 60 * 24);//获得当月最后一天的日期 
                   if (dateTpye == 'dayNum') {
                       return date_count;
                   }
                   else {
                       return last_date;
                   }
               },
               //获取两个时间相差天数-leo
               //计算sDate1 - sDate2 得到相差天数    2016-06-18格式  
               getdatedifference: function (sDate1, sDate2) {
                   var aDate, oDate1, oDate2, iDays
                   aDate = sDate1.split("-")
                   oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])    //转换为12-18-2006格式  
                   aDate = sDate2.split("-")
                   oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])
                   iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24)    //把相差的毫秒数转换为天数  
                   return iDays
               },
               //比较两个时间大小
               checkEndTime: function (startTime, endTime) {
                   var starttime = String(startTime).replace("-", "/").replace("-", "/")
                   var start = new Date(starttime);
                   var endtime = String(endTime).replace("-", "/").replace("-", "/")
                   var end = new Date(endtime);
                   if (end < start) {
                       return "Small";
                   }
                   else if (end > start) {
                       return "large";
                   }
                   else {
                       return "equal";
                   }
               }

           },

           /*转换*/
           convert: {
               //还原json格式的时间
               jsonReductionDate: function (cellval, format) {
                   try {
                       if (cellval == "" || cellval == null) return "";
                       var date = new Date(parseInt(cellval.substr(6)));
                       if (format == null) {
                           var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                           var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                           return date.getFullYear() + "-" + month + "-" + currentDate;
                       } else {
                           return $.convert.toDate(date, format);
                       }
                   } catch (e) {
                       return "";
                   }
               },
               jsonToStr: function (object) {
                   var type = typeof object;
                   if ('object' == type) {
                       if (Array == object.constructor) type = 'array';
                       else if (RegExp == object.constructor) type = 'regexp';
                       else type = 'object';
                   }
                   switch (type) {
                       case 'undefined':
                       case 'function':
                       case 'unknown':
                           return;
                           break;
                       case 'function':
                       case 'boolean':
                       case 'regexp':
                           return object.toString();
                           break;
                       case 'number':
                           return isFinite(object) ? object.toString() : 'null';
                           break;
                       case 'string':
                           return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g, function () {
                               var a = arguments[0];
                               return (a == '\n') ? '\\n' : (a == '\r') ? '\\r' : (a == '\t') ? '\\t' : ""
                           }) + '"';
                           break;
                       case 'object':
                           if (object === null) return 'null';
                           var results = [];
                           for (var property in object) {
                               var value = jQuery.convert.jsonToStr(object[property]);
                               if (value !== undefined) results.push(jQuery.convert.jsonToStr(property) + ':' + value);
                           }
                           return '{' + results.join(',') + '}';
                           break;
                       case 'array':
                           var results = [];
                           for (var i = 0; i < object.length; i++) {
                               var value = jQuery.convert.jsonToStr(object[i]);
                               if (value !== undefined) results.push(value);
                           }
                           return '[' + results.join(',') + ']';
                           break;
                   }
               },
               strToJson: function (str) {
                   return jQuery.parseJSON(str);
               },
               toDate: function (date, format) {
                   var data = new Date(date);
                   var o = {
                       "M+": data.getMonth() + 1, //month
                       "d+": data.getDate(), //day
                       "h+": data.getHours(), //hour
                       // "H+": date.getHours(), //hour
                       "m+": data.getMinutes(), //minute
                       "s+": data.getSeconds(), //second
                       "q+": Math.floor((data.getMonth() + 3) / 3), //quarter
                       "S": data.getMilliseconds() //millisecond
                   }
                   if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
                   (data.getFullYear() + "").substr(4 - RegExp.$1.length));
                   for (var k in o) if (new RegExp("(" + k + ")").test(format))
                       format = format.replace(RegExp.$1,
                   RegExp.$1.length == 1 ? o[k] :
                   ("00" + o[k]).substr(("" + o[k]).length));
                   return format;
               },
               toInt: function (par) {
                   if (par == null || par == NaN || par == "") return 0;
                   return parseInt(par);
               },
               toNumber: function (obj, pointNum) {
                   if ($.valiData.isDecimal(obj)) {
                       var num = parseFloat(obj) + "";
                       if (num.lastIndexOf(".") == -1) {
                           return parseFloat(num);
                       } else {
                           var index = num.indexOf(".");
                           var length = num.length;
                           if ((length - index - 1) > pointNum) {
                               return parseFloat(parseFloat(num).toFixed(pointNum));
                           } else {
                               return parseFloat(num);
                           }
                       }
                   } else {
                       return 0;
                   }
               },
               toFloat: function (par) {
                   if (par == null || par == NaN || par == "") return 0;
                   return parseFloat(par);
               },
               xmlToJQuery: function (data) {
                   var xml;
                   if ($.browser.msie) {// & parseInt($.browser.version) < 9
                       xml = new ActiveXObject("Microsoft.XMLDOM");
                       xml.async = false;
                       xml.loadXML(data);
                       // xml = $(xml).children('nodes'); //这里的nodes为最顶级的节点
                   } else {
                       xml = data;
                   }
                   return $(xml);
               },
               //将标准时间转换成时间格式-leo
               //day: Thu Aug 22 2013 15:12:00 GMT+0800 (中国标准时间)   format: yyyy-MM-dd hh:mm:ss
               standardTimeToDateTime: function (day, format) {
                   var dateTime = new Date(day);
                   var tostr = function (i) {
                       return (i < 10 ? '0' : '') + i
                   };
                   return format.replace(/yyyy|MM|dd|HH|mm|ss/g, function (item) {
                       switch (item) {
                           case 'yyyy':
                               return tostr(dateTime.getFullYear());
                               break;
                           case 'MM':
                               return tostr(dateTime.getMonth() + 1);
                               break;
                           case 'mm':
                               return tostr(dateTime.getMinutes());
                               break;
                           case 'dd':
                               return tostr(dateTime.getDate());
                               break;
                           case 'HH':
                               return tostr(dateTime.getHours());
                               break;
                           case 'ss':
                               return tostr(dateTime.getSeconds());
                               break;
                       }
                   })
               }
           },

           /*数据验证*/
           valiData: {
               isEmpty: function (val) { return val == undefined || val == null || val == "" || val.toString() == ""; },
               isZero: function (val) { return val == null || val == "" || val == 0 || val == "0"; },
               //判断是否为数字
               isNumber: function (val) { return (/^\d+$/.test(val)); },
               //是否是邮箱
               isMail: function (val) { return (/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(val)); },
               //是否是手机
               isMobilePhone: function (val) { return (/\d{11}$/.test(val)); },
               //判断是否为负数和整数
               isNumberOrNegative: function (val) { return (/^\d+|\-\d+$/.test(val)); },
               //金额验证
               isMoney: function (val) { return (/^[1-9]d*.d*|0.d*[1-9]d*|\d+$/.test(val)); },
               isDecimal: function (val) { return (/^(-?\d+)(\.\d+)?$/.test(val)); }

           },

           /*类型验证*/
           valiType: {
               isArray: function (obj) { return (typeof obj == 'object') && obj.constructor == Array; },
               isString: function (str) { return (typeof str == 'string') && str.constructor == String; },
               isDate: function (obj) { return (typeof obj == 'object') && obj.constructor == Date; },
               isFunction: function (obj) { return (typeof obj == 'function') && obj.constructor == Function; },
               isArrayLike: function (obj) {
                   if (obj == null || isWindow(obj)) {
                       return false;
                   }
                   var length = "length" in Object(obj) && obj.length;

                   if (obj.nodeType === NODE_TYPE_ELEMENT && length) {
                       return true;
                   }

                   return isString(obj) || isArray(obj) || length === 0 ||
                          typeof length === 'number' && length > 0 && (length - 1) in obj;
               },
               isObject: function (obj) { return (typeof obj == 'object') && obj.constructor == Object; }
           },

           pageHelper: {
               referenceFile: function (url, type) {
                   $(function () {
                       var isJs = type == "js";
                       if (isJs) {
                           var isAny = $("[src='" + url + "']").size() > 0;
                           if (!isAny)
                               $("head").append("<script src='" + url + "' /> ");
                       }
                       else {
                           var isAny = $("[href='" + url + "']").size() > 0;
                           if (!isAny)
                               $("head").append("<link href='" + url + "'rel='stylesheet' >");
                       }
                   })
               }
           },

           //定位
           position: {
               //使页面元素上下左右居中
               center: function (eleSelector) {
                   var obj = $(eleSelector);
                   if (obj.size() > 0) {
                       obj.each(function () {
                           var obj = $(this);
                           var wh = $(window).height();
                           var ww = $(window).width();
                           var scrh = $(document).scrollTop();
                           var objh = obj.height();
                           var objw = obj.width();
                           var top = scrh + ((wh - objh) / 2);
                           var left = ww / 2 - objw / 2;
                           if (scrh > 0) {
                               obj.css({ position: "absolute", left: left, top: top });
                           }
                       })
                   }
               }
           },

           //ajax辅助
           ajaxhelper: {
               error: function (msg, action) {
                   if (action != null) {
                       action(msg);
                   }
                   try {
                       console.log(msg);
                   } catch (e) {

                   }
               }
           },

           /*********************************浏览器操作*********************************/
           /*浏览获取操作*/
           request: {
               areaName: function () {
                   var infos = $.linq.where(window.location.href.replace($.request.domain(), "").split('/'), function (v) { return v != "" && v != "http:" && v != "https:" })
                   return infos[infos.length - 3];
               },
               controllerName: function () {
                   var infos = $.linq.where(window.location.href.replace($.request.domain(), "").split('/'), function (v) { return v != "" && v != "http:" && v != "https:" })
                   return infos[infos.length - 2];
               },
               actionName: function () {
                   var infos = $.linq.where(window.location.href.replace($.request.domain(), "").split('/'), function (v) { return v != "" && v != "http:" && v != "https:" })
                   var reval = infos[infos.length - 1];
                   if (reval != null) {
                       return reval.split('?')[0];
                   } else {
                       return null;
                   }
               },
               queryString: function () {
                   var s1;
                   var q = {}
                   var s = document.location.search.substring(1);
                   s = s.split("&");
                   for (var i = 0, l = s.length; i < l; i++) {
                       s1 = s[i].split("=");
                       if (s1.length > 1) {
                           var t = s1[1].replace(/\+/g, " ")
                           try {
                               q[s1[0]] = decodeURIComponent(t)
                           } catch (e) {
                               q[s1[0]] = unescape(t)
                           }
                       }
                   }
                   return q;
               },
               url: function () {
                   return window.location.href;
               },
               urlEncode: function (str) {
                   if (str == null) return "";
                   var tempstr = str.replace(/\+/g, encodeURI("%2B"));
                   return tempstr;
               },
               domain: function () {
                   return window.location.host;
               },
               pageName: function () {
                   var a = location.href;
                   var b = a.split("/");
                   var c = b.slice(b.length - 1, b.length).toString(String).split(".");
                   return c.slice(0, 1);
               },
               pageFullName: function () {
                   var strUrl = location.href;
                   var arrUrl = strUrl.split("/");
                   var strPage = arrUrl[arrUrl.length - 1];
                   return strPage;
               },
               back: function () {
                   history.go(-1);
               },
               getCookie: function (cookieName) {
                   var cookieValue = document.cookie;
                   var cookieStartAt = cookieValue.indexOf("" + cookieName + "=");
                   if (cookieStartAt == -1) {
                       cookieStartAt = cookieValue.indexOf(cookieName + "=");
                   }
                   if (cookieStartAt == -1) {
                       cookieValue = null;
                   }
                   else {
                       cookieStartAt = cookieValue.indexOf("=", cookieStartAt) + 1;
                       cookieEndAt = cookieValue.indexOf(";", cookieStartAt);
                       if (cookieEndAt == -1) {
                           cookieEndAt = cookieValue.length;
                       }
                       cookieValue = unescape(cookieValue.substring(cookieStartAt, cookieEndAt));//解码latin-1  
                   }
                   return cookieValue;
               },
               //打印
               print: function (id/*需要打印的最外层元素ID*/) {
                   var el = document.getElementById(id);
                   var iframe = document.createElement('IFRAME');
                   var doc = null;
                   iframe.setAttribute('style', 'position:absolute;width:0px;height:0px;left:-500px;top:-500px;');
                   document.body.appendChild(iframe);
                   doc = iframe.contentWindow.document;
                   doc.write('<div>' + el.innerHTML + '</div>');
                   doc.close();
                   iframe.contentWindow.focus();
                   iframe.contentWindow.print();
                   if (navigator.userAgent.indexOf("MSIE") > 0) {
                       document.body.removeChild(iframe);
                   }
               },
               //加入收藏夹
               addFavorite: function (surl, stitle) {
                   try {
                       window.external.addFavorite(surl, stitle);
                   } catch (e) {
                       try {
                           window.sidebar.addpanel(stitle, surl, "");
                       } catch (e) {
                           alert("加入收藏失败,请使用ctrl+d进行添加");
                       }
                   }
               },
               //设为首页
               setHome: function (obj, vrl) {
                   try {
                       obj.style.behavior = 'url(#default#homepage)';
                       obj.sethomepage(vrl);
                   } catch (e) {
                       if (window.netscape) {
                           try {
                               netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                           } catch (e) {
                               alert("此操作被浏览器拒绝!\n请在浏览器地址栏输入'about:config'并回车\n然后将[signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
                           }
                       } else {
                           alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动设置为首页。");
                       }
                   }
               }
           },

           /*浏览器请求操作*/
           response: {
               setCookie: function (name, value, time) {
                   if (time == null) {
                       time = 30 * 60 * 1000
                   }
                   //设置名称为name,值为value的Cookie
                   var expdate = new Date();   //初始化时间
                   expdate.setTime(expdate.getTime() + time);   //时间
                   document.cookie = name + "=" + value + ";expires=" + expdate.toGMTString() + ";path=/";

                   //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
               },
               open: function (url, params) {
                   if (params == null || params == "") {
                       window.open(url);
                   } else {
                       if (jQuery.linq.contains(url.toString(), "?")) {
                           var rurl = url + "&" + jQuery.param(params);
                           window.open(rurl);
                       } else {
                           var rurl = url + "?" + jQuery.param(params);
                           window.open(rurl);
                       }
                   }
               },
               //页面跳转
               redirect: function (url, params) {
                   if (params == null || params == "") {
                       window.location.href = url;
                   } else {
                       if (jQuery.linq.contains(url.toString(), "?")) {
                           var rurl = url + "&" + jQuery.param(params);
                           window.location.href = rurl;
                       } else {
                           var rurl = url + "?" + jQuery.param(params);
                           window.location.href = rurl;
                       }
                   }
               }

           },

           /*浏览器判段*/
           broVali: {
               //jquery1.9以上只需要判段IE
               isIE: function () {
                   if (!!window.ActiveXObject || "ActiveXObject" in window)
                       return true;
                   else
                       return false;
               },
               //老版本jquery用下面的函数
               isIE6: function () {
                   var flag = false;
                   if ($.browser.msie && $.browser.version == "6.0")
                       flag = true;
                   return flag;
               },
               isIE7: function () {
                   var flag = false;
                   if ($.browser.msie && $.browser.version == "7.0")
                       flag = true;
                   return flag;
               },
               isIE8: function () {
                   var flag = false;
                   if ($.browser.msie && $.browser.version == "8.0")
                       flag = true;
                   return flag;
               },
               isIE9: function () {
                   var flag = false;
                   if ($.browser.msie && $.browser.version == "9.0")
                       flag = true;
                   return flag;
               },
               isIE10: function () {
                   var flag = false;
                   if ($.browser.msie && $.browser.version == "10.0")
                       flag = true;
                   return flag;
               },
               isIE11: function () {
                   var flag = false;
                   if ($.browser.msie && $.browser.version == "11.0")
                       flag = true;
                   return flag;
               },
               isMozilla: function () {
                   var flag = false;
                   if ($.browser.mozilla)
                       flag = true;
                   return flag;
               },
               isOpera: function () {
                   var flag = false;
                   if ($.browser.opera)
                       flag = true;
                   return flag;
               },
               isSafri: function () {
                   var flag = false;
                   if ($.browser.safari)
                       flag = true;
                   return flag;
               },
               isMobile: function () {
                   var userAgentInfo = navigator.userAgent;
                   var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                   var flag = false;
                   for (var v = 0; v < Agents.length; v++) {
                       if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = true; break; }
                   }

                   return flag;
               },
               isIPhone: function () {
                   var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                   return jQuery.jQueryAny(Agents, function (v) {
                       return v == "iPhone";
                   });
               },
               isAndroid: function () {
                   var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
                   return jQuery.jQueryAny(Agents, function (v) {
                       return v == "Android";
                   });
               }
           }

       });
 

       /*********************************form操作*********************************/
       jQuery.fn.extend({
           //获取元素属性以","隔开
           attrToStr: function (attr) {
               var reval = "";
               this.each(function () {
                   reval += jQuery(this).attr(attr) + ","
               })
               reval = jQuery.jQueryAction.trimEnd(reval, ",");
               return reval;
           },
           //清空表单
           formClear: function () {
               this.find("input:text,select,input:hidden,input:password").each(function () {
                   $(this).val("");
               });
               this.find("input:checkbox,input:radio").each(function () {
                   $(this).removeAttr("checked");
               });
           },
           //将json对象自动填充到表单 
           //例如 $('form').formFill({data:{id:1},prefix:"user."}) 填充后  <input name='user.id' value='1' >
           formFill: function (option) {
               var prefix = option.prefix;
               if (prefix == undefined) prefix = "";
               var frmData = option.data;
               for (i in frmData) {
                   var dataKey = i;
                   var thisData = this.find("[name='" + prefix + i + "']");
                   var text = "text";
                   var hidden = "hidden";
                   if (thisData != null) {
                       var thisDataType = thisData.attr("type");
                       var val = frmData[i];
                       var isdata = (val != null && val.toString().lastIndexOf("/Date(") != -1);
                       if (thisDataType == "radio") {
                           thisData.filter("[value=" + val + "]").attr("checked", "checked")
                           if (val == true || val == "0") val = "True";
                           else if (val == false || val != "0") val = "False";
                           thisData.filter("[value=" + val + "]").not("donbool").attr("checked", "checked")
                       } else if (thisDataType == "checkbox") {
                           if (thisData.size() == 1) {
                               if (val == "true" || val == 1 || val == "True" || val == "1") {
                                   thisData.attr("checked", "checked");
                               } else {
                                   thisData.removeAttr("checked");
                               }
                           } else {

                               thisData.removeAttr("checked");
                               var cbIndex = i;
                               if (val.lastIndexOf(",") == -1) {
                                   this.find("[name='" + prefix + dataKey + "'][value='" + prefix + val + "']").attr("checked", "checked");
                               } else {
                                   jQuery(val.split(',')).each(function (i, v) {
                                       this.find("[name='" + prefix + dataKey + "'][value='" + prefix + v + "']").attr("checked", "checked");;
                                   })
                               }
                           }

                       } else {
                           if (isdata) {
                               val = jQuery.Convert.jsonReductionDate(val);
                           }
                           if (val == "null" || val == null)
                               val = "";
                           if (val == "" && thisData.attr("watertitle") == thisData.val()) {
                           } else {
                               thisData.val(val + "");
                               thisData.removeClass("watertitle")
                           }
                       }
                   }

               }

           }


       });


       /*********************************通用属性扩展*****************************/
       jQuery.ejqInit = function () {
           String.prototype.ejq_TryToLower = function () {
               if (this == null) return "";
               return this.toString().toLocaleLowerCase();
           }
           String.prototype.ejq_format = function (args) {
               var _dic = typeof args === "object" ? args : arguments;
               var reval = this.replace(/\{([^{}]+)\}/g, function (str, key) {
                   return _dic[key];
               });
               return reval;
           }
           String.prototype.ejq_append = function (args) {
               return this + args;
           }
           String.prototype.ejq_appendFormat = function (appendValue, appendArgs) {
               return this + appendValue.ejq_format(appendArgs);
           }
           String.prototype.ejq_selector = function (args) {
               return $(this);
           }
           String.prototype.ejq_toFixed = Number.prototype.ejq_toFixed = function (d) {
               var s = this + ""; if (!d) d = 0;
               if (s.indexOf(".") == -1) s += "."; s += new Array(d + 1).join("0");
               if (new RegExp("^(-|\\+)?(\\d+(\\.\\d{0," + (d + 1) + "})?)\\d*$").test(s)) {
                   var s = "0" + RegExp.$2, pm = RegExp.$1, a = RegExp.$3.length, b = true;
                   if (a == d + 2) {
                       a = s.match(/\d/g); if (parseInt(a[a.length - 1]) > 4) {
                           for (var i = a.length - 2; i >= 0; i--) {
                               a[i] = parseInt(a[i]) + 1;
                               if (a[i] == 10) { a[i] = 0; b = i != 1; } else break;
                           }
                       }
                       s = a.join("").replace(new RegExp("(\\d+)(\\d{" + d + "})\\d$"), "$1.$2");
                   } if (b) s = s.substr(1); return (pm + s).replace(/\.$/, "");
               } return this + "";
           };
           //获取字符串的实际长度(注:中文字符=2长度，英文字符=1长度)
           String.prototype.ejq_len = function () {
               var args = this;
               if (args != undefined && args != null && args != "") {
                   args = $.trim(args);
               }
               var revel = 0, len = args.length, charCode = -1;
               for (var i = 0; i < len; i++) {
                   charCode = args.charCodeAt(i);
                   if (charCode >= 0 && charCode <= 128)
                       revel += 1;
                   else
                       revel += 2;
               }
               return revel;
           };
           //截取指定长度的字符串(忽略中英文)，如果给定的字符串大于指定长度，截取指定长度返回，否者返回源字符串.
           String.prototype.ejq_cutstr = function (len) {
               var args = this;
               if (args != undefined && args != null && args != "") {
                   args = $.trim(args);
               }
               if (len == undefined || len == null || len == "") {
                   len = 0;
               }
               var str_length = 0;
               var str_len = 0;
               str_cut = new String();
               str_len = args.length;
               for (var i = 0; i < str_len; i++) {
                   var a = args.charAt(i);
                   str_length++;
                   if (escape(a).length > 4) {
                       //中文字符的长度经编码之后大于4  
                       str_length++;
                   }
                   str_cut = str_cut.concat(a);
                   if (str_length >= len) {
                       str_cut = str_cut.concat("...");
                       return str_cut;
                   }
               }
               //如果给定字符串小于指定长度，则返回源字符串；  
               if (str_length < len) {
                   return args;
               }
           };
       }
       jQuery.ejqInit();

   })(window, jQuery)
    exports('ejq', jQuery);
});