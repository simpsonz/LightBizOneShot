$.datepicker.setDefaults({
    dateFormat: 'yy-mm-dd'
    ,monthNames: ['년 1월','년 2월','년 3월','년 4월','년 5월','년 6월','년 7월','년 8월','년 9월','년 10월','년 11월','년 12월']
	,monthNamesShort: ['1월','2월','3월','4월','5월','6월','7월','8월','9월','10월','11월','12월']
    ,dayNamesMin: ['일', '월', '화', '수', '목', '금', '토']
	,showMonthAfterYear:true
	,changeYear:true
	,changeMonth:true
});
$(document).ready(function() {
	if ( $(".tableUlWrap").length ) tableTdOdd();
	if ( $(".basicTable").length ) basicTable();
	if ( $(".btnSearchBox").length ) boxToggle();
	if ( $(".detailSearch").length ) detailToggle();
	if ( $(".detailView").length ) detailView();
	if ( $(".mypageList").length ) tableTdEven();
	if ( $(".policyAgree").length ) policyView(".policyAgree");
	if ( $(".policyGuide").length ) policyView(".policyGuide");
	if ( $(".tab_t1View").length ) tab_t1View(".tab_t1View");
	
	if ( $(".popupWrap").length ) layerPop();
	$( ".firstDate" ).datepicker();
	$( ".lastDate" ).datepicker();
	$( ".taxDate" ).datepicker();
	$("#mainImg").bnnrRolling({ auto : true, speed : 3000 });
});