

function tableTdOdd() {
	$('.tableUlWrap ul li:odd').css({'background' : '#f6f6f6'});
}

function tableTdEven(name) {
	$('ul.mypageList>li:nth-child(odd)').css({'background' : '#f6f6f6'});
}

function basicTable() {
	$('.basicTable tr:odd').css({'background' : '#f6f6f6'});
}

function boxToggle() {
	var box = $('.btnSearchBox');
	var SearchBox = $('.searchBox');
	
	box.click(function(){
		SearchBox.toggle();	
		$(this).children('a').toggleClass('open');
		$(this).toggleClass('close');
		return false;
	});
	
}
function detailToggle(){
	var btn = $('.detailSearch');
	var cont = $('.detailTable');

	btn.click(function(){
		cont.toggle();
		return false;
	});
}

function detailView(){
	var btn = $('.detailView a');

	btn.live('click', function(){
		var cont = $(this).parent().children('div');
		if(cont.css("display") == "none"){
			$(this).addClass('on');
			cont.show();
		}
		else{
			$(this).removeClass('on');
			cont.hide();
		}
		return false;
	});
}

function policyView(obj){
	var btn = $(obj).children('ul').find('a');
	var prev = btn[0];
	
	btn.click(function(){
		$(prev).removeClass('on');
		$(this).addClass('on');
		$($(prev).attr("href")).css({"z-index":"1", "visibility":"hidden"});
		$($(this).attr("href")).css({"z-index":"2", "visibility":"visible"});
		prev = this;
		return false;
	});
}

function tab_t1View(obj){
	var btn = $(obj).children('ul').find('a');
	var prev = btn[0];
	
	btn.click(function(){
		$(prev).removeClass('tab_on');
		$(this).addClass('tab_on');
		$($(prev).attr("href")).css({"z-index":"1", "display":"none"});
		$($(this).attr("href")).css({"z-index":"2", "display":"block"});
		prev = this;
		return false;
	});
}


///main banner
$.fn.bnnrRolling = function( _options ){
	return this.each(function(i, n){
		var options = $.extend({}, { lists :".list li" ,anchors : ".anchors button" ,btnL : ".left", btnR : ".right", btnAuto : ".auto", speed : 3000, auto : false }, _options)

			, $wrap = $(this)
			, $anchors = $wrap.find( options.anchors )
			, $btnL = $wrap.find( options.btnL )
			, $btnR = $wrap.find( options.btnR )
			, $btnAuto = $wrap.find( options.btnAuto )
			, $lists = $wrap.find( options.lists )
		
			, active = 0
			, timer
			, auto = ( options.auto ) ? true : false
			, pause = false
			, speed = options.speed;
			
		// event
		$anchors.each(function(i){
			$(this).bind("click mouseover", function(){
				activeMenu(i);
				return false;
			});
		});

		$btnL.bind("click", btnLClick );
		$btnR.bind("click", btnRClick);
		$btnAuto.bind("click", btnAutoClick );
		
		// initialize
		$lists.hide();
		show(active);
		
		if( auto ){
			$wrap.bind("mouseenter focusin", wrapMouseEnter );
			$wrap.bind("mouseleave focusout", wrapMouseLeave );
			timerStart();
		}
		
		// function
		function btnLClick(){
			activeMenu("left");
			return false;
		}
		
		function btnRClick(){
			activeMenu("right");
			return false;
		}
		
		function btnAutoClick(){
			pause = ! pause;
			$btnAuto.toggleClass("stop", pause);
		}
		
		function activeMenu(n){
			if( active === n ) return;
			
			hide( active );

			 if( n == "left" ){			
				active--;
				if( active < 0 ) active = $lists.length - 1;
				
			}else if( n == "right" ){
				active++;
				if( active == $lists.length ) active = 0;
				
			}else if( typeof n === "number" ){
				active = n;
			}
			
			show( active );
		}
		
		// 배너 보이기/감추기
		function show( num ){
			$anchors.eq( num ).parent().addClass("current");
			$lists.eq( num ).show();
		}
		
		function hide( num ){
			$lists.eq( num ).hide();
			$anchors.eq( num ).parent().removeClass("current");
		}

		// 자동롤링관련함수
		function timerStart(){
			clearInterval( timer );
			timer = setInterval(function(){
				if( ! pause ) btnRClick();
			}, speed);
		}		
		function wrapMouseEnter(){
			clearInterval( timer );
		}
		function wrapMouseLeave(){
			timerStart();
		}
		
	});
}

//layer popup
function layerPop(){
	var $layerPopup = $('.popupWrap');
	var left = ( $(window).scrollLeft() + ($(window).width() - $layerPopup.width()) / 2 );
	var top = '300px';
	var layerOpen = $('.layerOpen');
	var layerClose = $('.closeLayer a');

	$layerPopup.css({'left':left,'top':top});
	$('body').css('position','relative').append($layerPopup);
	
	layerOpen.click(function(){
		$layerPopup.show();
		return false;
	});

	layerClose.click(function(){
		$layerPopup.hide();
		return false;
	});
}