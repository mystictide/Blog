var KodLoading = {
    open: function (duration) {
        $('.kod-loading').fadeIn(duration == undefined ? 500 : duration);
    },
    close: function (duration) {

        try {
            $('.kod-loading').fadeOut(duration == undefined ? 500 : duration);
        }
        catch (err) {

        }
        finally {
            $('.kod-loading').hide();
        }

    },
    refresh: function (url) {

        KodLoading.open();
        if (url == undefined || url == null) {
            location.reload();
        } else {
            window.location.href = url;
        }
    },
    progress: function (percent) {
        if ($('.kod-loading .percent').is(':visible')) {
            $('.kod-loading .percent').html(percent);
        } else {
            $('.kod-loading').append('<div class="percent">' + percent + '</div>');
        }
    }
};

var Filter = {

    Page: function (page) {
        var params = Filter.querystring.add("page", page, true);
        Filter.querystring.redirect(params);
    },

    Clear: function () {
        let querystring = Filter.querystring.read();
        KodLoading.refresh(window.location.pathname + "?keyword=" + querystring.get('keyword'));
    },

    querystring: {
        read: function () {
            return new URLSearchParams(window.location.search);
        },
        add: function (key, value, isChange) {
            var urlParams = Filter.querystring.read();
            if (isChange) {
                urlParams.delete(key);
                urlParams.append(key, value);
            } else {
                var newParams = new URLSearchParams();

                var control = false;
                for (pair of urlParams.entries()) {
                    if (pair[0] == key & pair[1] == value) {
                        control = true;
                    } else {
                        newParams.append(pair[0], pair[1]);
                    }
                }
                if (!control) {
                    newParams.append(key, value);
                }
                urlParams = newParams;
            }
            return urlParams;
        },
        redirect: function (newParams) {
            KodLoading.refresh(window.location.pathname + "?" + decodeURIComponent(newParams.toString()));
        }
    },

    Search: function (button) {
        //Formu buluyoruz. 
        var form = $(button).parents('form');
        //Mevcut url okuyoruz.
        var url = window.location.pathname;
        //İlk parametreyi bulabilmek için değişken tanımlıyoruz.
        var first = true;
        //Formun içindeki inputlarda dönüyoruz. 
        $('input', form).each(function (index, item) {
            //İnputun name alanını alıyoruz.
            var key = $(item).attr('name');
            //İnputun değerini alıyoruz.
            var value = $(item).val();
            //İnput dolumu kontrol ediyoruz.
            if (!(!value)) {
                //İlk parametre ise ? işaretini değilse & işaretini url ekliyoruz. 
                var _icon = first ? "?" : "&";
                url += _icon + key + "=" + value;
                first = false;
            }
        });
        //Formun içindeki param etiketli alanlarda dönüyoruz.
        $('.param.active', form).each(function (index, item) {
            //name alanını alıyoruz.
            var key = $(item).attr('name');
            //değerini alıyoruz.
            var value = $(item).data('id');
            //dolumu kontrol ediyoruz.
            if (!(!value) || value == 0) {
                //İlk parametre ise ? işaretini değilse & işaretini url ekliyoruz. 
                var _icon = first ? "?" : "&";
                url += _icon + key + "=" + value;
                first = false;
            }

        });

        //Yapılandırdığımız urlye gönderiyoruz.
        KodLoading.refresh(url);
    }

}