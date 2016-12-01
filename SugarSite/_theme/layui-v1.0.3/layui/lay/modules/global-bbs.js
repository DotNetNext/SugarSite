layui.define(['layer'], function (exports) {
    var $ = layui.jquery;
    //查看代码
    $(function () {
        var w = $(window).width();
        var $top = $(".layui-fixbar-top");
        $top.click(function () {
            $(document).scrollTop(0);
        })
        $(document).scroll(function () {
            var dt = $(document).scrollTop();
            if (dt > 0) {
                $top.show();
            } else {
                $top.hide();
            }
        })

    });
    exports('global', {});
});