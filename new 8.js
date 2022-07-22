var ie4 = (document.all) ? true : false;
var ns6 = (document.getElementById) ? true && !ie4 : false;
var opera_browser = (window.opera) ? true : false;
var KHTML_browser = (navigator.vendor == 'KDE') || (document.childNodes && !document.all && !navigator.taintEnabled);
var _numScrolls = 0;
var _scrolls = new Array();
var _dragObject;
var _thumbLayer; // contains thumb for scrolling
var _barLayer; // contains the scroll bar
var _scrollLayer; // points to the content that is scrolled
var _scrollInt;
var _scrollImgs;
var _nav4 = (navigator.appName.indexOf("Netscape") != -1);
var _loading = false;
var _domain = document.domain;

function fFrame(left, top, width, height, src, baseHREF, borderColor) {
    this.top = top;
    this.left = left;
    this.width = width;
    this.height = height;
    this.src = src;
    this.baseHREF = baseHREF;
    this.borderColor = borderColor || "#DDDDDD";
    this.inSync = new Array();
    this.id = _numScrolls;
    this.load = _fLoadSrc;
    this.scrollTo = _fScrollTo;
    this.scrollBy = _fScrollBy;
    this.init = _fInit;
    this.setup = _fSetup;
    if (src.indexOf("http://") != -1) {
        if (src.indexOf(_domain) == -1) {
            alert("fframe.js: Sorry, the src you specified is not in this domain.");
            return;
        }
    }
    if (_nav4) {
        this.isInit = false;
        this.isSetup = false;
        if (!baseHREF) {
            this.baseHREF = GURLBase + "/gfx/sb_";
        } else
            this.baseHREF = baseHREF;
        if (!_scrollImgs) _fLoadImages(this.baseHREF);
        this.init();
        this.load(false, false);
        this.setup();
    } else {
        document.body.innerHTML +=
            '<IFRAME WIDTH=' + width + ' HEIGHT=' + height + ' SCROLL=AUTO ' +
            'NAME="scroll' + _numScrolls + '" ' +
            'ID="scroll' + _numScrolls + '" ' +
            'STYLE="position:absolute;' +
            'left:' + left + ';' +
            'top:' + top + ';' +
            'width:' + width + ';' +
            'height:' + height + ';" ' +
            'SRC="' + src + '">' +
            '</IFRAME>\n';
        this.layer = document.all["scroll" + _numScrolls];
        this.layer.style.borderColor = this.borderColor;
        this.isInit = true;
        this.isSetup = true;
    }
    _numScrolls++;
    _scrolls[this.id] = this;
}

function _fLoadImages(baseHREF) {
    if (_loading) {
        setTimeout(_fLoadImages, 100, baseHREF);
        return;
    }
    _loading = true;
    _scrollImgs = new Array(13);
    _scrollImgs[0] = new Image(16, 16);
    _scrollImgs[0].src = baseHREF + "vup.gif";
    _scrollImgs[1] = new Image(16, 16);
    _scrollImgs[1].src = baseHREF + "vup2.gif";
    _scrollImgs[2] = new Image(16, 16);
    _scrollImgs[2].src = baseHREF + "vdown.gif";
    _scrollImgs[3] = new Image(16, 16);
    _scrollImgs[3].src = baseHREF + "vdown2.gif";
    _scrollImgs[4] = new Image(16, 16);
    _scrollImgs[4].src = baseHREF + "hleft.gif";
    _scrollImgs[5] = new Image(16, 16);
    _scrollImgs[5].src = baseHREF + "hleft2.gif";
    _scrollImgs[6] = new Image(16, 16);
    _scrollImgs[6].src = baseHREF + "hright.gif";
    _scrollImgs[7] = new Image(16, 16);
    _scrollImgs[7].src = baseHREF + "hright2.gif";
    _scrollImgs[8] = new Image(16, 16);
    _scrollImgs[8].src = baseHREF + "vbg.gif";
    _scrollImgs[9] = new Image(16, 17);
    _scrollImgs[9].src = baseHREF + "vthumb.gif";
    _scrollImgs[10] = new Image(16, 16);
    _scrollImgs[10].src = baseHREF + "hbg.gif";
    _scrollImgs[11] = new Image(17, 16);
    _scrollImgs[11].src = baseHREF + "hthumb.gif";
    _scrollImgs[12] = new Image(15, 15);
    _scrollImgs[12].src = baseHREF + "null.gif";
    _loading = false;
}

function _fInit() {
    _fRealInit(this);
}

function _fRealInit(frame) {
    if (_loading) {
        setTimeout(_fRealInit, 100, frame);
        return;
    }
    _loading = true;
    var borderLayer, contentLayer;
    borderLayer = new Layer(frame.width + 2);
    borderLayer.left = frame.left - 1;
    borderLayer.top = frame.top - 1;
    borderLayer.clip.width = frame.width + 2;
    borderLayer.height = frame.height + 2;
    borderLayer.clip.height = frame.height + 2;
    borderLayer.document.bgColor = frame.borderColor;
    borderLayer.visibility = "inherit";
    frame.borderLayer = borderLayer;
    contentLayer = new Layer(frame.width);
    contentLayer.captureEvents(Event.ONLOAD);
    contentLayer.onLoad = _fOnLoad;
    frame.layer = contentLayer;
    frame.layer.frame = frame;
    frame.vspLayer = new Layer(16); // contains scroll bar
    frame.vsuLayer = new Layer(16); // contains up arrow
    frame.vsdLayer = new Layer(16); // contains down arrow
    frame.vssLayer = new Layer(16); // contains thumb
    _fInitComp(frame.vspLayer,
        _scrollStartVJump, _scrollEndVJump, 8);
    _fInitComp(frame.vsuLayer,
        _scrollStartUp, _scrollEndUp, 0);
    _fInitComp(frame.vsdLayer,
        _scrollStartDown, _scrollEndDown, 2);
    _fInitComp(frame.vssLayer,
        _scrollStartDrag, _scrollEndDrag, 9);
    frame.vssLayer.parLayer = frame.layer;
    frame.vssLayer.spLayer = frame.vspLayer;
    frame.vspLayer.ssLayer = frame.vssLayer;
    frame.vsuLayer.ssLayer = frame.vssLayer;
    frame.vsdLayer.ssLayer = frame.vssLayer;
    frame.hspLayer = new Layer(frame.width - 29); // contains scroll bar
    frame.hsuLayer = new Layer(16); // contains left arrow
    frame.hsdLayer = new Layer(16); // contains right arrow
    frame.hssLayer = new Layer(17); // contains thumb
    _fInitComp(frame.hspLayer,
        _scrollStartHJump, _scrollEndHJump, 10);
    _fInitComp(frame.hsuLayer,
        _scrollStartLeft, _scrollEndLeft, 4);
    _fInitComp(frame.hsdLayer,
        _scrollStartRight, _scrollEndRight, 6);
    _fInitComp(frame.hssLayer,
        _scrollStartDrag, _scrollEndDrag, 11);
    frame.hssLayer.parLayer = frame.layer;
    frame.hssLayer.spLayer = frame.hspLayer;
    frame.hspLayer.ssLayer = frame.hssLayer;
    frame.hsuLayer.ssLayer = frame.hssLayer;
    frame.hsdLayer.ssLayer = frame.hssLayer;
    frame.nLayer = new Layer(15); // when two scroll bars
    _fInitComp(frame.nLayer,
        _fDoNothing, _fDoNothing, 12);
    frame.isInit = true;
    _loading = false;
}

function _fLoadSrc(src, noSetup) {
    _fRealLoadSrc(this, src, noSetup);
}

function _fRealLoadSrc(frame, src, noSetup) {
    if (_loading || !frame.isInit) {
        setTimeout(_fRealLoadSrc, 100, frame, src);
        return;
    }
    src = src || frame.src;
    frame.src = src;
    if (_nav4) {
        _loading = true;
        frame.layer.load(src, frame.width);
        if (frame.isSetup) {
            frame.layer.vScroll(0, false, true);
            frame.layer.hScroll(0, false, true);
        }
        if (!noSetup) frame.setup();
    } else {
        frame.layer.src = src;
    }
}

function _fSetup() {
    _fRealSetup(this);
}

function _fRealSetup(frame) {
    if (!_nav4) return;
    if (_loading || !frame.isInit) {
        setTimeout(_fRealSetup, 100, frame);
        return;
    }
    frame.hMax = frame.layer.document.width;
    frame.vMax = frame.layer.document.height;
    if ((frame.hMax > frame.width) && !frame.hasHScroll) {
        frame.hasHScroll = true;
        frame.height -= 16; // make content shorter to fit horiz. scroll bar
        frame.load(); // reload to see if hMax changes
        return;
    } else if ((frame.hMax <= frame.width) && frame.hasHScroll) {
        frame.hasHScroll = false;
        frame.height += 16; // make content taller
        frame.load(); // reload to see if hMax changes
        return;
    }
    if ((frame.vMax > frame.height) && !frame.hasVScroll) {
        frame.hasVScroll = true;
        frame.width -= 16; // make content skinnier to fit vert. scroll bar
        frame.load(); // reload to see if vMax changes
        return;
    } else if ((frame.vMax <= frame.height) && frame.hasVScroll) {
        frame.hasVScroll = false;
        frame.width += 16; // make content fatter
        frame.load(); // reload to see if hMax changes
        return;
    }
    frame.layer.left = frame.left;
    frame.layer.top = frame.top;
    frame.layer.clip.top = 0;
    frame.layer.clip.left = 0;
    frame.layer.clip.width = frame.width;
    frame.layer.clip.height = frame.height;
    frame.layer.bgColor = "#FFFFFF";
    frame.layer.visibility = "show";
    frame.layer.oHeight = frame.height;
    frame.layer.oWidth = frame.width;
    frame.layer.oTop = frame.top;
    frame.layer.oLeft = frame.left;
    frame.layer.vMax = frame.vMax;
    frame.layer.hMax = frame.hMax;
    frame.layer.vScroll = _fVScroll;
    frame.layer.hScroll = _fHScroll;
    frame.vssLayer.v = frame.hasVScroll;
    frame.hssLayer.h = frame.hasHScroll;
    if (frame.hasVScroll) {
        var vspLayerBottom = frame.height - 31;
        var vspLayerTop = frame.top + 16;
        var vsdLayerTop = frame.top + frame.height - 16;
        if (frame.hasVScroll) {
            vspLayerBottom++;
            vsdLayerTop++;
        }
        if (frame.baseHREF.indexOf("mac") != -1) {
            vspLayerTop--;
            vspLayerBottom++;
        }
        _fMoveComp(frame.vspLayer,
            vspLayerTop, frame.left + frame.width, 16, vspLayerBottom);
        _fMoveComp(frame.vsuLayer,
            frame.top, frame.vspLayer.left, 16, 16);
        _fMoveComp(frame.vsdLayer,
            vsdLayerTop, frame.vspLayer.left, 16, 16);
        _fMoveComp(frame.vssLayer,
            vspLayerTop, frame.vspLayer.left, 16, 17);
    } else {
        frame.vssLayer.visibility = "hide";
        frame.vspLayer.visibility = "hide";
        frame.vsuLayer.visibility = "hide";
        frame.vsdLayer.visibility = "hide";
    }
    if (frame.hasHScroll) {
        var hspLayerWidth = frame.width - 31;
        var hspLayerLeft = frame.left + 16;
        var hsdLayerLeft = frame.left + frame.width - 16;
        if (frame.hasVScroll) {
            hspLayerWidth++;
            hsdLayerLeft++;
        }
        if (frame.baseHREF.indexOf("mac") != -1) {
            hspLayerWidth++;
            hspLayerLeft--;
        }
        _fMoveComp(frame.hspLayer,
            frame.top + frame.height, hspLayerLeft, hspLayerWidth, 16);
        _fMoveComp(frame.hsuLayer,
            frame.hspLayer.top, frame.left, 16, 16);
        _fMoveComp(frame.hsdLayer,
            frame.hspLayer.top, hsdLayerLeft, 16, 16);
        _fMoveComp(frame.hssLayer,
            frame.hspLayer.top, hspLayerLeft, 17, 16);
    } else {
        frame.hssLayer.visibility = "hide";
        frame.hspLayer.visibility = "hide";
        frame.hsuLayer.visibility = "hide";
        frame.hsdLayer.visibility = "hide";
    }
    if (frame.hasHScroll || frame.hasVScroll) {
        document.captureEvents(Event.MOUSEMOVE);
        document.onmousemove = _scrollDrag;
    }
    if (frame.hasHScroll && frame.hasVScroll)
        _fMoveComp(frame.nLayer,
            frame.top + frame.height + 1, frame.left + frame.width + 1, 15, 15);
    else
        frame.nLayer.visibility = "hide";
    frame.isSetup = true;
}

function _fInitComp(layer, onmousedown, onmouseup, bgSrc) {
    layer.captureEvents(Event.MOUSEDOWN | Event.MOUSEUP);
    layer.onmousedown = onmousedown;
    layer.onmouseup = onmouseup;
    layer.background.src = _scrollImgs[bgSrc].src;
}

function _fMoveComp(layer, top, left, width, height) {
    layer.top = top;
    layer.left = left;
    layer.clip.width = width;
    layer.clip.height = height;
    layer.visibility = "inherit";
}

function _fDoNothing() {}

function _fOnLoad() {
    var i, out;
    _loading = false;
    for (i = 0; i < this.document.links.length; i++)
        out = this.document.links[i];
    this.captureEvents(Event.ONLOAD | Event.MOUSEDOWN);
    this.onLoad = _fOnLoad;
    this.onMouseDown = _fOnMouseDown;
}

function _fOnMouseDown(e) {
    if (e && this) {
        if (e.target && this.frame) {
            if (e.target.href) {
                if ((e.target.href.indexOf("mailto:") == -1) &&
                    (e.target.hostname == _domain) &&
                    ((e.target.target == null) ||
                        (e.target.target == "_self"))) { // no TARGET specified
                    this.frame.load(e.target);
                    return false;
                }
            }
        }
    }
    return true;
}

function _fScrollTo(x, y) {
    if (!this) return;
    if (!_nav4) {
        document.frames("scroll" + this.id).self.scrollTo(x, y);
        return;
    }
    if ((typeof(x) == "number") && (this.hMax > this.width)) {
        var p = x / (this.hMax - this.width);
        this.layer.hScroll(p, true);
    }
    if ((typeof(y) == "number") && (this.vMax > this.height)) {
        var p = y / (this.vMax - this.height);
        this.layer.vScroll(p, true);
    }
}

function _fScrollBy(x, y) {
    if (!this) return;
    if (!_nav4) {
        document.frames("scroll" + this.id).self.scrollBy(x, y);
        return;
    }
    if (typeof(x) == "number") {
        x += this.layer.clip.left;
        var p = x / (this.hMax - this.width);
        this.layer.hScroll(p, true);
    }
    if (typeof(y) == "number") {
        y += this.layer.clip.top;
        var p = y / (this.vMax - this.height);
        this.layer.vScroll(p, true);
    }
}

function _fVScroll(p, update, sync) {
    if (p > 1) p = 1;
    if (p < 0) p = 0;
    i = p * (this.vMax - this.oHeight);
    this.clip.top = i;
    this.clip.bottom = this.oHeight + i;
    this.clip.height = this.oHeight;
    this.top = this.oTop - i;
    if (update) {
        var frame = this.frame;
        frame.vssLayer.top = p * (frame.vspLayer.clip.height -
                frame.vssLayer.clip.height) +
            frame.vspLayer.top;
    }
    if (!sync) {
        for (f in this.frame.inSync) {
            f = this.frame.inSync[f];
            if (f.hMax > f.width) {
                p = i / (f.vMax - f.height);
                f.layer.vScroll(p, true, true);
            }
        }
    }
}

function _fHScroll(p, update, sync) {
    if (p > 1) p = 1;
    else if (p < 0) p = 0;
    i = p * (this.hMax - this.oWidth);
    this.clip.left = i;
    this.clip.right = this.oWidth + i;
    this.clip.width = this.oWidth;
    this.left = this.oLeft - i;
    if (update) {
        var frame = this.frame;
        frame.hssLayer.left = p * (frame.hspLayer.clip.width -
                frame.hssLayer.clip.width) +
            frame.hspLayer.left;
    }
    if (!sync) {
        for (f in this.frame.inSync) {
            f = this.frame.inSync[f];
            if (f.hMax > f.width) {
                p = i / (f.hMax - f.width);
                f.layer.hScroll(p, true, true);
            }
        }
    }
}

function _scrollStartDown(e) {
    _lastScrolledId = this.ssLayer.parLayer.frame.id;
    _thumbLayer = this.ssLayer;
    _barLayer = this.ssLayer.spLayer;
    _scrollLayer = this.ssLayer.parLayer;
    this.background.src = _scrollImgs[3].src;
    _scrollDown();
    _scrollInt = setInterval("_scrollDown();", 20);
    return false;
}

function _scrollDown() {
    p = (_scrollLayer.clip.top + 15) /
        (_scrollLayer.vMax - _scrollLayer.oHeight);
    _scrollLayer.vScroll(p, true);
}

function _scrollEndDown(e) {
    this.background.src = _scrollImgs[2].src;
    clearInterval(_scrollInt);
    return false;
}

function _scrollStartUp(e) {
    _lastScrolledId = this.ssLayer.parLayer.frame.id;
    _thumbLayer = this.ssLayer;
    _barLayer = this.ssLayer.spLayer;
    _scrollLayer = this.ssLayer.parLayer;
    this.background.src = _scrollImgs[1].src;
    _scrollUp();
    _scrollInt = setInterval("_scrollUp();", 20);
    return false;
}

function _scrollUp() {
    p = (_scrollLayer.clip.top - 15) /
        (_scrollLayer.vMax - _scrollLayer.oHeight);
    _scrollLayer.vScroll(p, true);
}

function _scrollEndUp(e) {
    this.background.src = _scrollImgs[0].src;
    clearInterval(_scrollInt);
    return false;
}

function _scrollStartRight(e) {
    _lastScrolledId = this.ssLayer.parLayer.frame.id;
    _thumbLayer = this.ssLayer;
    _barLayer = this.ssLayer.spLayer;
    _scrollLayer = this.ssLayer.parLayer;
    this.background.src = _scrollImgs[7].src;
    _scrollRight();
    _scrollInt = setInterval("_scrollRight();", 20);
    return false;
}

function _scrollRight() {
    p = (_scrollLayer.clip.left + 15) /
        (_scrollLayer.hMax - _scrollLayer.oWidth);
    _scrollLayer.hScroll(p, true);
}

function _scrollEndRight(e) {
    this.background.src = _scrollImgs[6].src;
    clearInterval(_scrollInt);
    return false;
}

function _scrollStartLeft(e) {
    _lastScrolledId = this.ssLayer.parLayer.frame.id;
    _thumbLayer = this.ssLayer;
    _barLayer = this.ssLayer.spLayer;
    _scrollLayer = this.ssLayer.parLayer;
    this.background.src = _scrollImgs[5].src;
    _scrollLeft();
    _scrollInt = setInterval("_scrollLeft();", 20);
    return false;
}

function _scrollLeft() {
    p = (_scrollLayer.clip.left - 15) /
        (_scrollLayer.hMax - _scrollLayer.oWidth);
    _scrollLayer.hScroll(p, true);
}

function _scrollEndLeft(e) {
    this.background.src = _scrollImgs[4].src;
    clearInterval(_scrollInt);
    return false;
}

function _scrollStartVJump(e) {
    _lastScrolledId = this.ssLayer.parLayer.frame.id;
    _thumbLayer = this.ssLayer;
    _barLayer = this.ssLayer.spLayer;
    _scrollLayer = this.ssLayer.parLayer;
    if (e.pageY > this.ssLayer.top)
        dir = "1";
    else dir = "-1";
    _scrollVJump(dir, -1);
    _scrollInt = setInterval("_scrollVJump(" + dir + "," + e.pageY + ");", 50);
    return false;
}

function _scrollVJump(dir, py) {
    p = (_scrollLayer.clip.top + dir * _scrollLayer.clip.height) /
        (_scrollLayer.vMax - _scrollLayer.oHeight);
    if (p > 1) p = 1;
    else if (p < 0) p = 0;
    t = p * (_barLayer.clip.height - _thumbLayer.clip.height) +
        _barLayer.top;
    if ((dir == 1) && (py != -1) && (t > py)) return;
    if ((dir == -1) && (py != -1) && (t < (py - 17))) return;
    _scrollLayer.vScroll(p);
    _thumbLayer.top = t;
}

function _scrollEndVJump() {
    clearInterval(_scrollInt);
}

function _scrollStartHJump(e) {
    _lastScrolledId = this.ssLayer.parLayer.frame.id;
    _thumbLayer = this.ssLayer;
    _barLayer = this.ssLayer.spLayer;
    _scrollLayer = this.ssLayer.parLayer;
    if (e.pageX > this.ssLayer.left)
        dir = "1";
    else dir = "-1";
    _scrollHJump(dir, -1);
    _scrollInt = setInterval("_scrollHJump(" + dir + "," + e.pageX + ");", 50);
    return false;
}

function _scrollHJump(dir, px) {
    p = (_scrollLayer.clip.left + dir * _scrollLayer.clip.width) /
        (_scrollLayer.hMax - _scrollLayer.oWidth);
    if (p > 1) p = 1;
    if (p < 0) p = 0;
    t = p * (_barLayer.clip.width - _thumbLayer.clip.width) +
        _barLayer.left;
    if ((dir == 1) && (px != -1) && (t > px)) return;
    if ((dir == -1) && (px != -1) && (t < (px - 17))) return;
    _scrollLayer.hScroll(p);
    _thumbLayer.left = t;
}

function _scrollEndHJump() {
    clearInterval(_scrollInt);
}

function _scrollStartDrag(e) {
    if (this.parLayer) {
        _lastScrolledId = this.parLayer.frame.id;
        _dragObject = this;
        _dragObject.oTop = _dragObject.top;
        _dragObject.oLeft = _dragObject.left;
        _dragObject.hOffset = e.pageX - _dragObject.oLeft;
        _dragObject.vOffset = e.pageY - _dragObject.oTop;
        return false;
    }
    return true;
}

function _scrollDrag(e) {
    if (!_dragObject) return true;
    var pY = e.pageY;
    var pX = e.pageX;
    if (_dragObject.v) {
        pY -= _dragObject.vOffset;
        if ((pX > (_dragObject.left + 54)) ||
            (pX < (_dragObject.left - 38))) {
            pY = _dragObject.oTop;
        }
        if (pY < _dragObject.spLayer.top)
            pY = _dragObject.spLayer.top;
        if (pY > _dragObject.spLayer.clip.height +
            _dragObject.spLayer.top -
            _dragObject.clip.height)
            pY = _dragObject.spLayer.clip.height +
            _dragObject.spLayer.top -
            _dragObject.clip.height;
        _dragObject.top = pY;
        percentScroll = (pY - _dragObject.spLayer.top) /
            (_dragObject.spLayer.clip.height -
                _dragObject.clip.height);
        _dragObject.parLayer.vScroll(percentScroll);
    } else {
        pX -= _dragObject.hOffset;
        if ((pY > (_dragObject.top + 54)) ||
            (pY < (_dragObject.top - 38))) {
            pX = _dragObject.oLeft;
        }
        if (pX < _dragObject.spLayer.left)
            pX = _dragObject.spLayer.left;
        if (pX > _dragObject.spLayer.clip.width +
            _dragObject.spLayer.left -
            _dragObject.clip.width)
            pX = _dragObject.spLayer.clip.width +
            _dragObject.spLayer.left -
            _dragObject.clip.width;
        _dragObject.left = pX;
        percentScroll = (pX - _dragObject.spLayer.left) /
            (_dragObject.spLayer.clip.width -
                _dragObject.clip.width);
        _dragObject.parLayer.hScroll(percentScroll);
    }
    return false;
}

function _scrollEndDrag() {
    _dragObject = false;
    return false;
}
var GSubmitting = false;

function IWOnError(AMsg, AUrl, ALineNo) {
    GSubmitting = false;
    return false;
}
window.onError = IWOnError;

function ProcessElement(obj) {
    var i = 0;
    if (obj.name != null && obj.form && obj.form.name != "HiddenSubmitForm") {
        if (obj.name.length > 0) {
            dobj = GSubmitter.elements[obj.name];
            if (dobj != null && obj != dobj) {
                if (obj.type) {
                    if (obj.type == "select-one") {
                        if (obj.selectedIndex != -1) {
                            dobj.value = obj.options[obj.selectedIndex].value;
                        } else {
                            dobj.value = -1
                        }
                    } else
                    if (obj.type == "select-multiple") {
                        if (obj.selectedIndex != -1) {
                            dobj.value = dobj.value = obj.options[obj.selectedIndex].value + ',';
                            for (i = 0; i < obj.length; i++) {
                                if (obj.options[i].selected == true) {
                                    dobj.value = dobj.value + obj.options[i].value + ",";
                                }
                            }
                        } else {
                            dobj.value = -1
                        }
                    } else
                    if (obj.type == "checkbox") {
                        dobj.value = obj.checked ? "on" : "off";
                    } else
                    if (obj.type == "radio") {
                        if (obj.checked) {
                            dobj.value = obj.value;
                        }
                    } else
                    if (obj.type != "button") {
                        dobj.value = obj.value;
                    }
                }
            }
        }
    }
}

function LoadURL(URL) {
    location.replace(URL);
    return true;
}

function NewWindow(URL, Name, Options) {
    w = window.open(URL, Name, Options);
    return true;
}

function Status(msg) {
    window.status = msg;
    return true;
}

function CoolCheckBoxToggle(Name, Image, Submit) {
    InitSubmitter();
    var df = FindElem(Name);
    if (df.value == "On") {
        df.value = "Off";
        Image.src = GImageCache_CoolCheckBox_False.src;
    } else {
        df.value = "On";
        Image.src = GImageCache_CoolCheckBox_True.src;
    }
    if (Submit) {
        SubmitClick(Name, '', false);
    }
}
var ValidClick = true;

function SubmitClickConfirm(objname, param, ADoValidation, AConfirmation) {
    if (IsLocked()) return false;
    if (AConfirmation.length == 0) {
        if (GActivateLock) ActivateLock();
        return SubmitClick(objname, param, ADoValidation);
    } else if (!GSubmitting) {
        if (window.confirm(AConfirmation)) {
            if (GActivateLock) ActivateLock();
            return SubmitClick(objname, param, ADoValidation);
        } else {
            return true;
        }
    } else {
        return false;
    }
}

function SubmitClick(objname, param, ADoValidation) {
    if (GSubmitting == false) {
        GSubmitting = true;
        var i = 0;
        var j = 0;
        var AItem;
        if (document.activeElement) {
            GActiveControl = document.activeElement.id;
        } else {
            GActiveControl = null;
        }
        InitSubmitter();
        if (GSubmitter != null) {
            for (j = GSubmitter.elements.length - 1; j >= 0; j--) {
                var name = GSubmitter.elements[j].name;
                if ((name != "IW_Action") && (name != "IW_ActionParam") && (name != "IW_FormName") && (name != "IW_FormClass") && (name != " IW_width") && (name != "IW_height")) {
                    if (GSubmitter.elements[j].type != "hidden") { // Automatically submit all hidden fields
                        if (document.getElementsByName(name).length == 0) {
                            GSubmitter.removeChild(GSubmitter.elements[j]);
                        }
                    }
                }
            }
            for (j = 0; j < GSubmitter.elements.length; j++) {
                AItem = LocateInputElement(GSubmitter.elements[j].name, ProcessElement);
            }
        }
        if (ADoValidation == true) {
            if (Validate() == false) {
                GSubmitting = false;
                return true;
            }
        }
        SetBusy(true);
        GSubmitter.elements.IW_Action.value = objname;
        GSubmitter.elements.IW_ActionParam.value = param;
        GSubmitter.submit();
    }
    return true;
}

function layerWrite(id, nestref, text) {
    if (ns6) {
        document.getElementById(id).innerHTML = text;
    } else if (ie4) {
        document.all[id].innerHTML = text;
    }
}

function layerWriteAppend(id, nestref, text) {
    if (ns6) {
        document.getElementById(id).innerHTML += text;
    } else if (ie4) {
        document.all[id].innerHTML += text;
    }
}

function CheckReturnKey(AKey, AName, AValidation) {
    if (AKey == 13) {
        SubmitClick(AName, '', AValidation);
        return false;
    } else {
        return true;
    }
}
/*function AddHistoryToList(ALocation) {
if(!IWTop().IWHistoryList) {
IWTop().IWHistoryList = new Array();
}
if (IsBackButtonAction(ALocation)) {
IWTop().IWHistoryList = IWTop().IWHistoryList.slice(0, HistoryIndex(ALocation));
} else {
IWTop().IWHistoryList[IWTop().IWHistoryList.length] = ALocation;
}
}
function HistoryIndex(ALication) {
for (i = 0; i < IWTop().IWHistoryList.length; i++) {
if (IWTop().IWHistoryList[i] == ALocation) {
return i;
}
}
return -1;
}
function IsBackButtonAction(ALocation) {
if (IWTop().IWHistoryList) {
return (HistoryIndex(ALocation) > -1);
} else {
return false;
}
}*/
/* This function has been left as proxy
in case it's used by third-parties.
Changed on 19 Feb 2004 - Hadi
*/
function FindFocus(objname) {
    return FindElem(objname);
}
/*
This function is used to validate TIWTimeEdit components
*/
function ValidateTimeEdit(TimeEditName) {
    LEdit = FindElem(TimeEditName);
    if (LEdit) {
        LRegExp = new RegExp("(^[0-9]+(h|H|m|M|d|D|w|W)?$)|(^[0-9]+:[0-9]+(h|H)?$)|(^[0-9]+[\.][0-9]+(d|D|h|H|w|W){1}$)", "i");
        return LRegExp.test(LEdit.value);
    }
}

function SelectRadioButton(name, selected) {
    var elems = document.getElementsByName(name);
    var i;
    for (i = 0; i < elems.length; i++) {
        elems[i].checked = false;
    }
    FindElem(selected).checked = true;
}