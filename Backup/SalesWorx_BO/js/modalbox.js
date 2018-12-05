/*
 ModalBox - The pop-up window thingie with AJAX, based on prototype and script.aculo.us.

 Copyright Andrew Okonetchnikov (andrej.okonetschnikow@gmail.com), 2006
 All rights reserved.
 
 VERSION 1.5
 Last Modified: 02/02/2007
 
 Changelog:

ver 1.5: (02/02/2007)
 Added: 	URL parameters are now passing to AJAX.Request. Use postOptions to pass parameters
 Added: 	Loading message can be customized through options. Use loadingString option
 Added:		Script.aculo.us 1.64 and prototype 1.5rc1 support
 Added:		Callbacks added on first showing, updating, loading content, closing modalbox
 Added:		Callback can be passed through hide method
 Added: 	resize method resize modalbox without loading any content into it
 Changed:	Evaluating contained scripts (striping HTML comments)
 Changed: 	Appearing of overlay is now animated
 Changed: 	Attaching events on window and overlay
 Changed: 	Method hide now get the callbacks as a parameter
 Changed: 	Inititalization redone with Builder class
 Changed: 	Minor fixes and refactoring done
 Changed: 	Focus loop engine redone
 Changed: 	self variables replaced to bind(this) functions
 Fixed: 	Scrolling to initial scroll position after closing MB
 Fixed: 	Fixed bug in IE with body's overflow auto
 
ver 1.4: (06/20/2006)
 Added: 	Core definitions rewriten. Modalbox can now be accessed thorugh Modalbox object with public methods show and hide
 Added: 	License added
 Changed:	kbdHandler method is now public, so it can be stopped from other functions
 Fixed: 	Stopping of event observing in hide method
 Fixed: 	Hiding selects for IE issue (was applied on element ID)
 Removed:	Redundant 'globalMB' global variable removed
 Removed:	Scroll window events observerving
 Removed:	Redundant effect ScalyTo
 Issue: 	IE display bug then hidding scrollbars. Document body should have zero margins
 
 ver 1.3: (06/18/2006)
 Added: 	ModalBox will now get focus after opening
 Added: 	Keystrokes handler added (Tab key is looped on ModalBox and closing ModalBox by pressing Esc)
 Added: 	Window scrolling disabled (known issue: content jupms on top then opening ModalBox)
 Fixed: 	All dependent event handlers now unloads then closing ModalBox
 Fixed: 	SELECT element hiding function executes now only in MSIE
 Fixed: 	'Close' button has now href attribute to receive focus
 Fixed: 	Click on 'Close' button doesn't adds an href value to URL string
 
 ver 1.2: 
 Added: Global variable 'globalMB' added to the file. Use this variable to acces one instance of ModalBox and call methods on it
 
 ver 1.1: 
 Added: Added SELECT elements hiding for IE (should be rewriten later)
 
 ver 1.0: 
 Added: Core class description
 
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the David Spurr nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

* http://www.opensource.org/licenses/bsd-license.php
 
See scriptaculous.js for full scriptaculous licence

*/

if (!window.Modalbox)
	var Modalbox = new Object();

Modalbox.Methods = {
	focusableElements: new Array,
	
	setOptions: function(options) {
		this.options = {
			overlayClose: true, // Close modal box by clicking on overlay
			width: 400,
			height: 400,
			params: {},
			loadingString: "Please wait. Loading..."
		};
		Object.extend(this.options, options || {});
	},
	
	_init: function() {
		// Define there page content starts (first element after body)
		this.pageContent = document.body.childNodes[0];
		//Create the overlay
		this.MBoverlay = Builder.node("div", { id: "MB_overlay" });
		//Create the window
		this.MBwrapper = Builder.node("div", {id: "MB_wrapper"}, [
			this.MBwindow = Builder.node("div", {id: "MB_window", style: "display: none"}, [
				this.MBframe = Builder.node("div", {id: "MB_frame"}, [
					this.MBheader = Builder.node("div", {id: "MB_header"}, [
						this.MBcaption = Builder.node("div", {id: "MB_caption"}),
						this.MBclose = Builder.node("a", {id: "MB_close", title: "Close window", href: "#"}, [
							Builder.build("<span>&times;</span>"),
						]),
					]),
					this.MBcontent = Builder.node("div", {id: "MB_content"}, [
						this.MBloading = Builder.node("div", {id: "MB_loading"}, this.options.loadingString),
					]),
				]),
			]),
		]);
		// Inserting into DOM
		document.body.insertBefore(this.MBoverlay, this.pageContent);
		document.body.insertBefore(this.MBwrapper, this.pageContent);
		//Adding event observers
		this.hide = this.hide.bindAsEventListener(this);
		this.close = this._hide.bindAsEventListener(this);
		
		// Initial scrolling position of the window. To be used for remove scrolling effect during ModalBox appearing
		this.initScrollX = window.pageXOffset || document.body.scrollLeft || document.documentElement.scrollLeft;
		this.initScrollY = window.pageYOffset || document.body.scrollTop || document.documentElement.scrollTop;
		
		Event.observe(this.MBclose, "click", this.close); // Close link overver
		if(this.options.overlayClose)
			Event.observe(this.MBoverlay, "click", this.hide); // Overlay close obersver

		this.isInitialized = true; // Mark as initialized
	},
	
	show: function(title, url, options) {
		this.title = title;
		this.url = url;
		this.setOptions(options);
		
		if(!this.isInitialized) this._init(); // Check for is already initialized
		
		if (navigator.appVersion.match(/\bMSIE\b/))
		{
			document.body.style.position = "relative";
			document.body.style.height = document.documentElement.clientHeight + "px";
			document.body.style.width = document.documentElement.clientWidth + "px";
		}
		document.body.style.overflow = 'hidden';
		
		Element.update(this.MBcaption, title); // Updating title of the MB
		
		if(this.MBwindow.style.display == "none") { // First modal box appearing
			this._appear();
			this.event("onShow"); // Passing onShow callback
		}
		else { // If MB already on the screen, update it
			this._update();
			this.event("onUpdate"); // Passing onShow callback
		} 
	},
	
	hide: function(options) { // External hide method to use from external HTML and JS
		if(options) Object.extend(this.options, options); // Passing callbacks
		Effect.SlideUp(this.MBwindow, { duration: 0.35, afterFinish: this._deinit.bind(this) } );
	},
	
	_hide: function(event) { // Internal hide method to use inside MB class
		if(event) Event.stop(event);
		this.hide();
	},
	
	_appear: function() { // First appearing of MB
		this._toggleSelects();
		this._setOverlay();
		this._setWidth();
		this._setPosition();
		new Effect.Fade(this.MBoverlay, {from: 0, to: 0.75, duration: 0.5, afterFinish: function() {
				new Effect.SlideDown(this.MBwindow, {duration: 0.75, afterFinish: this.loadContent.bind(this) });
			}.bind(this)
		});
		this._setWidthAndPosition = this._setWidthAndPosition.bindAsEventListener(this);
		this.kbdHandler = this.kbdHandler.bindAsEventListener(this);
		Event.observe(window, "resize", this._setWidthAndPosition );
		Event.observe(document, "keypress", this.kbdHandler );
	},
	
	resize: function(byWidth, byHeight, options) { // Change size of MB without loading content
		if(options) Object.extend(this.options, options); // Passing callbacks
		this.currentDims = [this.MBwindow.offsetWidth, this.MBwindow.offsetHeight];
		new Effect.ScaleBy(this.MBwindow, 
			(byWidth), //New width calculation
			(byHeight), //New height calculation
			{ duration: .5, afterFinish: function() { this.event("afterResize") }.bind(this) // Passing callback
		});
	},
	
	_update: function() { // Updating MB in case of wizards
		this.currentDims = [this.MBwindow.offsetWidth, this.MBwindow.offsetHeight];
		if((this.options.width + 10 != this.currentDims[0]) || (this.options.height + 5 != this.currentDims[1]))
			new Effect.ScaleBy(this.MBwindow, 
							   (this.options.width + 10 - this.currentDims[0]), //New width calculation
							   (this.options.height + 5 - this.currentDims[1]), //New height calculation
								{afterFinish: this._loadAfterResize.bind(this), 
								beforeStart: function(effect) { effect.element.firstChild.childNodes[1].innerHTML = this.options.loadingString; effect.element.firstChild.childNodes[1].style.height = "auto"; }.bind(this) 
			});
		else {
			this.MBwindow.firstChild.childNodes[1].innerHTML = this.options.loadingString;
			this.MBwindow.firstChild.childNodes[1].style.height = "auto";
			this._loadAfterResize();
		}
	},
	
	loadContent: function () { // Load content into MB through AJAX
		var myAjax = new Ajax.Request( this.url, { method: 'get', parameters: this.options.params, 
			onComplete: function(originalRequest) {
				var response = new String(originalRequest.responseText);
				this.MBcontent.innerHTML = response;
				response.extractScripts().map(function(script) { 
					return eval(script.replace("<!--", "").replace("// -->", ""));
				});
				// If the ModalBox frame containes form elements or links, first of them will bi focused after loading content
				this.focusableElements = $A($("MB_content").descendants()).findAll(function(node){return (["INPUT", "TEXTAREA", "SELECT", "A", "BUTTON"].include(node.tagName));});
				this.moveFocus(this.focusableElements); // Setting focus on first 'focusable' element in content (input, select, textarea, link or button)
				this.event("afterLoad"); // Passing callback
			}.bind(this)
		});
	},
	
	moveFocus: function(elementsArray) { // Setting focus to be looped inside current MB
		if(elementsArray.length > 0)
			elementsArray[0].focus(); // Focus on first focusable element
		else
		{
			$("MB_close").focus();
			this.focusableElements[0] = $("MB_close");
		}
	},
	
	_loadAfterResize: function() {
		this._setWidth();
		this._setPosition();
		this.loadContent();
	},
	
	kbdHandler: function(e) {
		switch(e.keyCode)
		{
			case Event.KEY_TAB:
				if(Event.element(e) == this.focusableElements.last()) {
					Event.stop(e);
					this.moveFocus(this.focusableElements);  // Find last element in MB to handle event on it. If no elements found, uses close ModalBox button
				}
			break;			
			case Event.KEY_ESC:
				this._hide(e);
			break;
		}
	},
	
	_deinit: function()
	{	
		this._toggleSelects(); // Toggle back 'select' element in IE
		Event.stopObserving(this.MBclose, "click", this.close );
		if(this.options.overlayClose)
			Event.stopObserving(this.MBoverlay, "click", this.hide );
		Event.stopObserving(window, "resize", this._setWidthAndPosition );
		Event.stopObserving(document, "keypress", this.kbdHandler );
		Effect.toggle(this.MBoverlay, 'appear', {duration: 0.35, afterFinish: this._removeElements.bind(this) });
	},
	
	_removeElements: function () {
		if (navigator.appVersion.match(/\bMSIE\b/))
		{
			document.body.style.position = "";
			document.body.style.height = "";
			document.body.style.width = "";
		}
		document.body.style.overflow = "";
		window.scrollTo(this.initScrollX, this.initScrollY);
		Element.remove(this.MBoverlay);
		Element.remove(this.MBwrapper);
		this.isInitialized = false;
		this.event("afterHide"); // Passing afterHide callback
	},
	
	_setOverlay: function () {
		var array_page_size = this._getWindowSize();
		var max_height = Math.max(this._getScrollTop() + array_page_size[1], this._getScrollTop() + this.options.height + 30);
		this.MBoverlay.style.height = max_height + "px";
		this.MBoverlay.style.width = array_page_size[0] + "px";
	},
	
	_setWidth: function () {
		var array_page_size = this._getWindowSize();
		
		//Set size
		this.MBwrapper.style.width = this.options.width + 10 +"px";
		this.MBwindow.style.width = this.options.width + "px";
		
		this.MBwrapper.style.height = this.options.height + "px";
		this.MBwindow.style.height = this.options.height + "px";
		//this.MBcontent.style.height = this.options.height - 42 + "px";
	},
	
	_setPosition: function () {
		var array_page_size = this._getWindowSize();
		this.MBwrapper.style.left = ((array_page_size[0] - this.MBwrapper.offsetWidth) / 2 ) + "px";
		this.MBwindow.style.left = "0px";
		this.MBwrapper.style.top = this._getScrollTop() + "px";
	},
	
	_setWidthAndPosition: function () {
		this._setOverlay();
		this._setPosition();
	},
	
	_getWindowSize: function (){
		var window_width, window_height;
		if (self.innerHeight) {	// all except Explorer
			window_width = self.innerWidth;
			window_height = self.innerHeight;
		} else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
			window_width = document.documentElement.clientWidth;
			window_height = document.documentElement.clientHeight;
		} else if (document.body) { // other Explorers
			window_width = document.body.clientWidth;
			window_height = document.body.clientHeight;
		}
		return [window_width, window_height];
	},
	
	_getScrollTop: function () {
		//From: http://www.quirksmode.org/js/doctypes.html
		var theTop;
		if (document.documentElement && document.documentElement.scrollTop)
			theTop = document.documentElement.scrollTop;
		else if (document.body)
			theTop = document.body.scrollTop;
		return theTop;
	},
	// For IE browsers -- hiding all SELECT elements
	_toggleSelects: function() {
		if (navigator.appVersion.match(/\bMSIE\b/))
		{
			var selectsList = this.pageContent.getElementsByTagName("select");
			var selects = $A(selectsList);
			selects.each( function(select) { 
				select.style.visibility = (select.style.visibility == "") ? "hidden" : "";
			});
		}
	},
	event: function(eventName) {
		if(this.options[eventName]) this.options[eventName](this.obj);
	}
}

Object.extend(Modalbox, Modalbox.Methods);

Effect.ScaleBy = Class.create();
Object.extend(Object.extend(Effect.ScaleBy.prototype, Effect.Base.prototype), {
  initialize: function(element, byWidth, byHeight, options) {
    this.element = $(element)
    var options = Object.extend({
	  scaleFromTop: true,
      scaleMode: 'box',        // 'box' or 'contents' or {} with provided values
      scaleByWidth: byWidth,
	  scaleByHeight: byHeight
    }, arguments[3] || {});
    this.start(options);
  },
  setup: function() {
    this.elementPositioning = this.element.getStyle('position');
      
    this.originalTop  = this.element.offsetTop;
    this.originalLeft = this.element.offsetLeft;
	
    this.dims = null;
    if(this.options.scaleMode=='box')
      this.dims = [this.element.offsetHeight, this.element.offsetWidth];
	 if(/^content/.test(this.options.scaleMode))
      this.dims = [this.element.scrollHeight, this.element.scrollWidth];
    if(!this.dims)
      this.dims = [this.options.scaleMode.originalHeight,
                   this.options.scaleMode.originalWidth];
	  
	this.deltaY = this.options.scaleByHeight;
	this.deltaX = this.options.scaleByWidth;
  },
  update: function(position) {
    var currentHeight = this.dims[0] + (this.deltaY * position);
	var currentWidth = this.dims[1] + (this.deltaX * position);
	
    this.setDimensions(currentHeight, currentWidth);
  },

  setDimensions: function(height, width) {
    var d = {};
    d.width = width + 'px';
    d.height = height + 'px';
    
	var topd  = (height - this.dims[0])/2;
	var leftd = (width  - this.dims[1])/2;
	if(this.elementPositioning == 'absolute') {
		if(!this.options.scaleFromTop) d.top = this.originalTop-topd + 'px';
		d.left = this.originalLeft-leftd + 'px';
	} else {
		if(!this.options.scaleFromTop) d.top = -topd + 'px';
		d.left = -leftd + 'px';
	}
    this.element.setStyle(d);
  }
});