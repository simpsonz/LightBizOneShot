
/*기업회원 header show hide layer*/
jQuery(function($){
var layerWindow = $('#header_logoutwrap');
var layer = $('.header_logout');
// Show Hide
$('.name, .down').click(function(){
	if(layerWindow.hasClass('open')){
		layerWindow.removeClass('open');
	}else{
		layerWindow.addClass('open');
	}
});

// ESC Event
$(document).keydown(function(event){
if(event.keyCode != 27) return true;
if (layerWindow.hasClass('open')) {
layerWindow.removeClass('open');
}
return false;
 
});
// Hide Window
layerWindow.find('> .empty').mousedown(function(event){
layerWindow.removeClass('open');
return false;
});
});


/*세무회계사회원 header show hide layer*/
jQuery(function($){
var layerWindow = $('#header_logoutwrap_account');
var layer = $('.header_logout');
// Show Hide
$('.name, .down').click(function(){
  if(layerWindow.hasClass('open')){
    layerWindow.removeClass('open');
  }else{
    layerWindow.addClass('open');
  }
});

// ESC Event
$(document).keydown(function(event){
if(event.keyCode != 27) return true;
if (layerWindow.hasClass('open')) {
layerWindow.removeClass('open');
}
return false;
 
});
// Hide Window
layerWindow.find('> .empty').mousedown(function(event){
layerWindow.removeClass('open');
return false;
});
});


/*세무회계사회원 직원신규등록 tab*/
// Wait until the DOM has loaded before querying the document
$(document).ready(function(){
$('.tabs_area').each(function(){
  // For each set of tabs, we want to keep track of
  // which tab is active and it's associated content
  var $active, $content, $links = $(this).find('a');

  // If the location.hash matches one of the links, use that as the active tab.
  // If no match is found, use the first link as the initial active tab.
  $active = $($links.filter('[href="'+location.hash+'"]')[0] || $links[0]);
  $active.addClass('on');

  $content = $($active[0].hash);

  // Hide the remaining content
  $links.not($active).each(function () {
    $(this.hash).hide();
  });

  // Bind the click event handler
  $(this).on('click', 'a', function(e){
    // Make the old tab inactive.
    $active.removeClass('on');
    $content.hide();

    // Update the variables with the new link and content
    $active = $(this);
    $content = $(this.hash);

    // Make the tab active.
    $active.addClass('on');
    $content.show();

    // Prevent the anchor's default click action
    e.preventDefault();
  });
});
}); 


/* 팝업창 띄우기 */
function openWin(_url, _width, _height) { 
      window.open(_url, "popup", "width="+_width+", height="+_height+", resizable=no, scrollbars=no, menubar=no, toobar=no, status=no, location=no, ") ; 
  } 
  /* menubar yes/no : 윈도우 menubar 표시여부 / toolbar yes/no : 윈도우 toolbar 표시여부 
  location yes/no : 윈도우 location box 표시여부 / directories yes/no : 윈도우 directory button들의 표시여부 
  status yes/no : 윈도우 상태표시바 표시여부 / scrollbars yes/no : 윈도우 scrollbar 표시여부 
  resizable yes/no : 윈도우 크기가 조정될 수 있는 지 여부 / width : 윈도우 너비 / height : 윈도우 높이   */

function popupMsg(msg) {
  alert(msg);
  confirm(msg);
}