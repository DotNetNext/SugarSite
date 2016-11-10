layui.define(['layer', 'code', 'form', 'element', 'util'], function (exports) {
    var $ = layui.jquery
    , layer = layui.layer
    , form = layui.form()
    , util = layui.util
    , device = layui.device();

    //查看代码
    $(function () {
        var sc = $(".site-content").height();
        var sb = $(".site-banner-bg").css({ height: sc });
        var $top = $(".layui-fixbar-top");
        var $tree = $(".layui-tree");
        $top.click(function () {
            $(document).scrollTop(0);
        })
        $(document).scroll(function () {
            var wh = $(window).height();
            var dt = $(document).scrollTop();
            if (dt > wh) {
                $top.show();
                $tree.css("position", "fixed");
                $tree.css("top", "0");
            } else {
                $top.hide();
                $tree.removeAttr("style");
            }
        })
    });
    exports('global', {});
});