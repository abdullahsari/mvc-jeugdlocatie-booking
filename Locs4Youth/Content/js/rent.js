$(function () {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        startDate: '0d',
        todayHighlight: true,
        weekStart: 1
    });
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month'
        },
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: site_url + "/api/SharedRental/" + loc_id,
                type: "get",
                success: function (data) {
                    $.each(data, function (key, value) {
                        value['end'] = value['end'].split("T")[0] + "T24:00:00";
                    });
                    callback(data);
                }
            });
        },
        locale: 'nl-be',
        defaultView: 'month',
        aspectRatio: 1.5,
        textColor: 'gray',
        timezone: 'local',
        overlap: true,
        editable: false,
        selectable: true,
        selectHelper: true,
        eventRender: function (event, element) {
            element.append('<br />');
        }
    });
});