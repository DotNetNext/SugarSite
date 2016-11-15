layui.define(['layer', 'code', 'form', 'element', 'util'], function (exports) {
    var $ = layui.jquery
    , layer = layui.layer
    , form = layui.form()
    , util = layui.util
    , device = layui.device();

    //查看代码
    $(function () {
        var w = $(window).width();
        var body = $(".site-home");
        if (w < 1200) {
            body.removeClass("site-home")
        }
        var sc = $(".site-content").height();
        var sb = $(".site-banner-bg").css({ height: sc });
        var $top = $(".layui-fixbar-top");
        var $tree = $(".layui-main");
        var $more = $(".site-tree-mobile");
        var $shade = $(".site-mobile-shade");
        $top.click(function () {
            $(document).scrollTop(0);
        })
        $(document).scroll(function () {
            if ($tree.size() > 0) {
                var wh = $(window).height();
                var dt = $(document).scrollTop();
                if (dt > wh) {
                    $top.show();
                    if (w > 1200) {
                        $tree.addClass("site-fix");
                    }
                } else {
                    $top.hide();
                    $tree.removeClass("site-fix");
                }
            }
        })
        $more.click(function () {
            body.addClass("site-mobile");
        });
        $shade.click(function () {
            body.removeClass("site-mobile");
        });
    });
    exports('global', {});
});