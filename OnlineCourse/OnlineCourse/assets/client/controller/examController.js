var exam = {
    init: function () {
        exam.registerEvents();
    },
    registerEvents: function () {
        $('#btnStartQuiz').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var examid = btn.data('examid');
            var userid = btn.data('userid');
           
            $.ajax({
                url: "/Exam/AddResult",
                data: {
                    examid: examid,
                    userid: userid
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                       
                        //bootbox.alert({
                        //    message: "Bạn đã thêm bình luận thành công",
                        //    size: 'medium',
                        //    closeButton: false
                        //});
                       
                    }
                    else {
                        //bootbox.alert("Thêm bình luận lỗi");
                    }
                }
            });
        });
        $('#btnFinish').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var examid = btn.data('examfinishid');
            var userid = btn.data('userfinishid');
            var resultessay = CKEDITOR.instances['txtContent'].getData();
            var resultquiz = "";

            $('button span a').each(function () {
                if ($(this).hasClass('btn slide-btn circleblue'))
                    resultquiz += $(this).data('id') + ",";
            });
            resultquiz = resultquiz.substr(0, resultquiz.length - 1);

            var ContributeModel = {
                examid: examid,
                userid: userid,
                resultessay: resultessay,
                resultquiz : resultquiz
            }


            $.ajax({
                url: "/Exam/UpdateResult",
                data: JSON.stringify(ContributeModel),
                contentType: "application/json;charset=utf-8",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {

                        //bootbox.alert({
                        //    message: "Bạn đã thêm bình luận thành công",
                        //    size: 'medium',
                        //    closeButton: false
                        //});

                    }
                    else {
                        //bootbox.alert("Thêm bình luận lỗi");
                    }
                }
            });
        });


    }
}
exam.init();