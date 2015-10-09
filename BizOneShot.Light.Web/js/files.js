/*
<ul>
	<li><!--파일1-->
		<input type="text" class="file" readonly ="readonly" />
		<button type="button" class="file_search" value="찾아보기" onclick="return file_add(this);"></button>
		<button type="button" class="file_add" value="파일추가" onclick="return file_inpput_add(this);"></button>
		<button type="button" class="file_del" value="파일삭제" onclick="return file_inpput_remove(this);"></button>
		<em><input type="file" name="file" onchange="return get_filename(this);" /></em>
	</li>
</ul>
*/

// 첨부파일 추가
function file_add(e) {
    
    if ($(e).parent().find('em').find('input[type="file"]').val() == "") {
        $(e).parent().find('em').find('input[type="file"]').trigger('click');
    } else {
        $(e).parent().find('input.file').val('');
        $(e).parent().find('em').find('input[type="file"]').val('');
        //$(e).parent().find('.bt_td_delete').attr('class', 'file_search');

        var temp = $(e).parent().parent().find('li');
        if (temp.length < 2) {
            $(e).parent().parent().find('.bt_td_delete').attr('class', 'file_search');
            return false;
        }
        else {
            var $cloneADD = $('.file_add').clone();
            var $cloneDEL = $('.file_del').clone();
            $($cloneADD).insertAfter($(temp[temp.length - 2]).find('button.file_search'));
            $($cloneDEL).insertAfter($cloneADD);


            $($cloneADD).insertAfter($(temp[temp.length - 2]).find('button.bt_td_delete'));
            $($cloneDEL).insertAfter($cloneADD);

            $(e).parent().remove();
        }


    }
    return false;
}
// 첨부파일 파일명 추출
function get_filename(e) {
    var temp = $(e).val();

    if (temp != "") {
        var ext = temp.substring(temp.lastIndexOf(".") + 1).toLowerCase();
        var exts = ['doc', 'docs', 'docx', 'txt', 'xlsx', 'xls', 'pdf', 'ppt', 'pptx', 'hwp'];
        if (exts.indexOf(ext) < 0) {
            alert('문서 파일만 첨부가 가능합니다');
            return false;
        }
        $(e).parent().parent().find('input.file').val(temp.substring(temp.lastIndexOf("\\") + 1));
        $(e).parent().parent().find('.file_search').attr('class', 'bt_td_delete');
    }
    return false;
}

// 이미지 파일명 추출
function get_imagename(e) {
    var temp = $(e).val();

    if (temp != "") {
        var ext = temp.substring(temp.lastIndexOf(".") + 1).toLowerCase();
        var exts = ['jpg', 'jpeg', 'gif', 'bmp', 'png'];
        if (exts.indexOf(ext) < 0) {
            alert('이미지 파일만 첨부가 가능합니다');
            return false;
        }
        $(e).parent().parent().find('input.file').val(temp.substring(temp.lastIndexOf("\\") + 1));
        $(e).parent().parent().find('.file_search').attr('class', 'bt_td_delete');
    }
    return false;
}

// 첨부파일 입력칸추가
function file_inpput_add(e) {
    var temp = $(e).parent().parent().find('li');

    if (temp.length > 4) {
        alert('첨부파일은 최대 5개까지 가능합니다');
        return false;
    }
    var $cloneEL = $(e).parent().clone();
    $cloneEL.find('input.file').val('');
    $cloneEL.find('em').find('input[type="file"]').val('');
    $('.file_add').remove();
    $('.file_del').remove();
    $($cloneEL).find('.bt_td_delete').attr('class', 'file_search');
    $($cloneEL).insertAfter(temp[temp.length - 1]);
    return false;
}

// 첨부파일 입력칸 삭제
function file_inpput_remove(e) {
    var temp = $(e).parent().parent().find('li');

    if (temp.length < 2) return false;

    var $cloneADD = $('.file_add').clone();
    var $cloneDEL = $('.file_del').clone();

    if ($(temp[temp.length - 2]).find('file_add').length < 1) {
        $($cloneADD).insertAfter($(temp[temp.length - 2]).find('button.file_search'));
        $($cloneDEL).insertAfter($cloneADD);

        $($cloneADD).insertAfter($(temp[temp.length - 2]).find('button.bt_td_delete'));
        $($cloneDEL).insertAfter($cloneADD);
    }
    //$($cloneADD).insertAfter($(temp[temp.length - 2]).find('button.file_search'));
    //$($cloneDEL).insertAfter($cloneADD);

    //$($cloneADD).insertAfter($(temp[temp.length - 2]).find('button.bt_td_delete'));
    //$($cloneDEL).insertAfter($cloneADD);

    $(temp[temp.length - 1]).remove();

    return false;
}

//파일삭제
function file_inpput_remove2(e) {
    var temp = $(e).parent().remove();

    return false;
}
