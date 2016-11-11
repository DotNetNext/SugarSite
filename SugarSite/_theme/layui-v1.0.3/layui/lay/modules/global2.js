layui.define(['layer'], function (exports) {
    var $ = layui.jquery;
    $("button").click(function (e) {
          e.preventDefault();
    })
    var $sizeDemo = $(".site-demo");
    var $gridTable = $(".gridtable");
    if ($gridTable.size() > 0) {
        $gridTable.css({ width: $sizeDemo.width()-250})
    }
});