var options = {
    orderCellsTop: !0,
    fixedHeader: !0,
    initComplete: function () {
        var e = this.api(); e.columns().eq(0).each(function (t) {
            var o = $(".filters th").eq($(e.column(t).header()).index()), i = $(o).text(); $(e.column(t).header()).index() >= 1 ? $(o).html('<input type="text" class="form-control form-control-sm" placeholder="' + i + '"/>') : $(o).html(""), $("input", $(".filters th").eq($(e.column(t).header()).index())).off("keyup change").on("change", function (o) { $(this).attr("title", $(this).val()), this.selectionStart, e.column(t).search("" != this.value ? "({search})".replace("{search}", "(((" + this.value + ")))") : "", "" != this.value, "" == this.value).draw() }).on("keyup", function (e) { var t = this.selectionStart; e.stopPropagation(), $(this).trigger("change"), $(this).focus()[0].setSelectionRange(t, t) })
        })
    }, dom: "Bfrtip", buttons: [{ extend: 'csv', exportOptions: { columns: ':gt(0)' } }, 'pageLength']
};