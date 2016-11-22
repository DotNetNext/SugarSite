layui.define(function (exports) {
    var JSGrid = function (config) {
        this.selectedRow = -1;
        this.doubleHead = config.doubleHead != null ? config.doubleHead : false;
        this.rowSpan = config.rowSpan != null ? config.rowSpan : false;
        this.rowSpanKeys = config.rowSpanKeys != null ? config.rowSpanKeys : [];
        this.TempRowSpanKeys = [];
        this.width = config.width != null ? "width:" + config.width + "px;" : "";
        if (this.width != null) {
            this.width = this.width.replace("%px", "%");
        }
        this.height = config.height != null ? "height:" + config.height + "px;" : "";
        if (this.height != null) {
            this.height = this.height.replace("%px", "%");
        }
        this.id = config.id != null ? config.id : "TreGrid";
        this.columns = config.columns != null ? config.columns : [];
        this.dataStore = config.dataStore != null ? config.dataStore : [];
        this.LmainTable = document.createElement("table");
        this.RmainTable = document.createElement("table");
        this.LmainTable.id = this.id + '_mainLeftTB';
        this.LmainTable.className = 'InnerTB';
        this.RmainTable.id = this.id + '_mainRightTB';
        this.RmainTable.className = 'InnerTB';
        this.rowSpanGroupData = [];
    }
    JSGrid.prototype.getColumn = function (in_columns) {
        return {
            title: in_columns.title != null ? in_columns.title : "",
            indexname: in_columns.indexname != null ? in_columns.indexname : "",
            width: in_columns.width != null ? in_columns.width : 50,
            textalign: in_columns.textalign != null ? in_columns.textalign : "center",
            locked: in_columns.locked != null ? in_columns.locked : 50,
            describe: in_columns.describe != null ? in_columns.describe : 0,
            formatter: in_columns.formatter != null ? in_columns.formatter : function (val, rowdata, rowindex) {
                return val;
            },
            rowSpanRow: in_columns.rowSpanRow != null ? in_columns.rowSpanRow : false,
            doubleHeadNum: in_columns.doubleHeadNum != null ? in_columns.doubleHeadNum : 0,
            doubleHeadtitle: in_columns.doubleHeadtitle != null ? in_columns.doubleHeadtitle : ""
        }
    }
    JSGrid.prototype.CreateGridHead = function () {
        var Icolumns;
        var doubleRow = 0;
        var doubleRowspan = "";
        var doubleHead1 = '';
        var doubleHead2 = '';
        var Head1str = '<tr>';
        var Head2str = '<tr>';
        for (var i = 0; i < this.columns.length; i++) {
            Icolumns = this.getColumn(this.columns[i]);
            var width = Icolumns.width;
            if (this.doubleHead) {
                if (doubleRow <= 0 & Icolumns.doubleHeadNum > 0) {
                    doubleRow = Icolumns.doubleHeadNum;
                }
                if (doubleRow == 0 & Icolumns.doubleHeadNum > 0) {
                    doubleRow = Icolumns.doubleHeadNum;
                }
            }
            var headTitle = '<div gridName="' + Icolumns.indexname + '_' + i + '_head" style="width:'
             + width
             + 'px;text-align:'
             + Icolumns.textalign
             + ';text-overflow:ellipsis; white-space:nowrap; overflow:hidden;">'
             + Icolumns.title
             + '</div>';
            var tempstr = '<td class="tablehead" >' + headTitle + '</td>';
            if (this.doubleHead) {
                var doubletemp = '';
                tempstr = '<td rowspan=2 class="tablehead doubleHeadHeight">' + headTitle + '</td>'
                if (doubleRow == Icolumns.doubleHeadNum & doubleRow > 0) {
                    tempstr = '<td class="tablehead" colspan=' + Icolumns.doubleHeadNum + '>' + Icolumns.doubleHeadtitle + '</td>';
                    doubletemp = '<td class="tablehead">' + headTitle + '</td>';
                } else if (doubleRow > 0) {
                    tempstr = '';
                    doubletemp = '<td class="tablehead">' + headTitle + '</td>';
                }
                doubleRow = doubleRow - 1;
                if (Icolumns.locked) {
                    doubleHead1 = doubleHead1 + doubletemp
                } else {
                    doubleHead2 = doubleHead2 + doubletemp
                }
            }
            if (Icolumns.locked) {
                Head1str = Head1str + tempstr;
            } else {
                Head2str = Head2str + tempstr;
            }
        }
        doubleHead1 = '</tr>' + doubleHead1 + '</tr>';
        doubleHead2 = '</tr>' + doubleHead2 + '</tr>';
        Head1str = Head1str + '</tr>' + doubleHead1;
        Head2str += '<td style="border:0px;"><div style="width:18px;"/></td></tr>' + doubleHead2;
        return {
            HeadHTML1: Head1str,
            HeadHTML2: Head2str
        };
    }
    JSGrid.prototype.RenderTo = function (ControlID) {
        var id = this.id;
        var HeadArr = this.CreateGridHead();
        var gridHTML = ' <div id="' + id + '" style="overflow:hidden;' + this.width + this.height + '"> '
         + '<table id="' + id + '_alltable"  style="border-collapse:collapse;" >' + '<tr>'
         + '  <td class="allTD">' + '   <div id="' + id + '_headLeftDiv">   '
         + '     <table id="' + id + '_headLeftTB" class="' + (this.doubleHead ? 'doubleHead' : 'InnerTB') + '">'
         + HeadArr.HeadHTML1
         + '     </table>' + '   </div>' + '  </td>'
         + '  <td class="allTD">' + '   <div id="' + id + '_headRightDiv" style="overflow:hidden;">'
         + '     <table id="' + id + '_headRightTB" class="' + (this.doubleHead ? 'doubleHead' : 'InnerTB') + '">'
         + HeadArr.HeadHTML2
         + '     </table>' + ' </div> ' + '  </td>' + ' </tr>'
         + ' <tr>'
         + '  <td class="allTD">' + '   <div id="' + id + '_mainLeftDiv" style="overflow:hidden">   '
         + '   </div>' + '</td>'
         + '  <td class="allTD" >' + '   <div id="' + id + '_mainRightDiv" style="overflow-x:auto;overflow-y:auto;" >'
         + '</div>' + '</td>' + ' </tr>'
         + '</table>'
         + '</div>';
        document.getElementById(ControlID).innerHTML = gridHTML;
    }
    JSGrid.prototype.RowClick = function (tr, grid) {
        if (grid.selectedRow != -1) {
            var Lc = document.getElementById('_row_' + grid.selectedRow + '_' + grid.id).className;
            var Rc = document.getElementById('r_row_' + grid.selectedRow + '_' + grid.id).className;
            document.getElementById('_row_' + grid.selectedRow + '_' + grid.id).className = Lc.replace("selectRowLight", "");
            document.getElementById('r_row_' + grid.selectedRow + '_' + grid.id).className = Lc.replace("selectRowLight", "");
        }
        grid.selectedRow = tr.rn;
        var trL = document.getElementById('_row_' + tr.rn + '_' + grid.id);
        var trR = document.getElementById('r_row_' + tr.rn + '_' + grid.id);
        trL.className = trL.className + ' ' + 'selectRowLight';
        trR.className = trR.className + ' ' + 'selectRowLight';
        grid.onRowClick(tr, grid.selectedRow);
    };
    JSGrid.prototype.onRowClick = function (tr, rownum) { }
    JSGrid.prototype.CreatRow = function (rownum, dtSource) {
        var trL = this.LmainTable.insertRow(-1);
        var trR = this.RmainTable.insertRow(-1);
        trL.id = '_row_' + rownum + '_' + this.id;
        trR.id = 'r_row_' + rownum + '_' + this.id;
        var RowClick = this.RowClick;
        var myGrid = this;
        trL.onclick = function () {
            RowClick(trL, myGrid)
        };
        trR.onclick = function () {
            RowClick(trR, myGrid)
        };
        trL.rn = rownum;
        trR.rn = rownum;
        var Icolumns;
        var td;
        for (var j = 0; j < this.columns.length; j++) {
            Icolumns = this.getColumn(this.columns[j]);
            var tempstr = '';
            if (Icolumns.locked) {
                td = trL.insertCell(-1);
            } else {
                td = trR.insertCell(-1);
            }
            var tempValue = Icolumns.formatter(dtSource[rownum][Icolumns.indexname], dtSource[rownum], rownum, td, trL, trR);
            td.className = 'nomarl';
            tempstr = '<div gridName="' + Icolumns.indexname + '_' + j + '_body" style="width:'
             + Icolumns.width
             + "px;text-align:"
             + Icolumns.textalign
             + ";text-overflow:ellipsis; white-space:nowrap; overflow:hidden;\">"
             + tempValue
             + "</div>";
            td.innerHTML = tempstr;
        }
    }
    JSGrid.prototype.RowSpanCreatRow = function (rownum, dtSource) {
        var trL = this.LmainTable.insertRow(-1);
        var trR = this.RmainTable.insertRow(-1);
        trL.id = '_row_' + rownum + '_' + this.id;
        trR.id = 'r_row_' + rownum + '_' + this.id;
        var RowClick = this.RowClick;
        var myGrid = this;
        trL.onclick = function () {
            RowClick(trL, myGrid)
        };
        trR.onclick = function () {
            RowClick(trR, myGrid)
        };
        trL.rn = rownum;
        trR.rn = rownum;
        var Icolumns;
        var td;
        var tdStyle = 'top';
        for (var i = 0; i < this.rowSpanKeys.length; i++) {
            if (rownum <= 0) {
                continue;
            }
            tdStyle = 'mid';
            if (dtSource[rownum][this.rowSpanKeys[i]] != dtSource[rownum - 1][this.rowSpanKeys[i]]) {
                tdStyle = 'top';
                break;
            }
        }

        if (tdStyle == 'top') {
            this.rowSpanGroupData.push({ id: trL.id, rows: [] });
            trL.IsRowSpanned = false;
        } else {
            if (this.rowSpanGroupData.length > 0) {
                this.rowSpanGroupData[this.rowSpanGroupData.length - 1].rows.push(trL.id);
                this.rowSpanGroupData[this.rowSpanGroupData.length - 1].rows.push(trR.id);
                trL.IsRowSpanned = true;
            }
        }

        for (var j = 0; j < this.columns.length; j++) {
            Icolumns = this.getColumn(this.columns[j]);
            var tempstr = '';
            if (Icolumns.locked) {
                td = trL.insertCell(-1);
            } else {
                td = trR.insertCell(-1);
            }
            var tempValue = Icolumns.formatter(dtSource[rownum][Icolumns.indexname], dtSource[rownum], rownum, td, trL, trR);
            td.className = 'nomarl';
            if (Icolumns.rowSpanRow) {
                if (tdStyle == 'top') {
                    td.style.borderBottom = '0px';
                }
                if (tdStyle == 'mid') {
                    td.style.borderBottom = '0px';
                    td.style.borderTop = '0px';
                    tempValue = '';
                }
                if ((dtSource.length - 1) == rownum) {
                    td.style.borderBottom = '1px solid #d6d6d6';
                }
            }
            tempstr = '<div gridName="' + Icolumns.indexname + '_' + j + '_body" style="width:'
             + Icolumns.width
             + "px;text-align:"
             + Icolumns.textalign
             + ";text-overflow:ellipsis; white-space:nowrap; overflow:hidden;\">"
             + tempValue
             + "</div>";
            td.innerHTML = tempstr;
        }
    }
    JSGrid.prototype.creatNullRow = function () {
        var trL = this.LmainTable.insertRow(-1);
        var trR = this.RmainTable.insertRow(-1);
        trL.id = '_row_Null_' + this.id;
        trR.id = 'r_row_Null_' + this.id;
        var Icolumns;
        var td;
        for (var j = 0; j < this.columns.length; j++) {
            Icolumns = this.getColumn(this.columns[j]);
            var tempstr = '';
            if (Icolumns.locked) {
                td = trL.insertCell(-1);
            } else {
                td = trR.insertCell(-1);
            }
            tempstr = '<div gridName="' + Icolumns.indexname + '_' + j + '_body" style="width:'
             + Icolumns.width
             + "px;text-overflow:ellipsis; white-space:nowrap; overflow:hidden;\"></div>";
            td.innerHTML = tempstr;
            td.style.border = '1px transparent solid';
        }
    }
    JSGrid.prototype.ShowData = function () {
        this.rowSpanGroupData = [];
        this.LmainTable = document.createElement("table");
        this.RmainTable = document.createElement("table");
        this.LmainTable.id = this.id + '_mainLeftTB';
        this.LmainTable.className = 'InnerTB';
        this.RmainTable.id = this.id + '_mainRightTB';
        this.RmainTable.className = 'InnerTB';
        this.selectedRow = -1;
        document.getElementById(this.id + '_mainLeftDiv').innerHTML = '';
        document.getElementById(this.id + '_mainRightDiv').innerHTML = '';
        if (this.dataStore.length == 0) {
            this.creatNullRow()
        }
        for (var i = 0; i < this.dataStore.length; i++) {
            if (this.rowSpan) {
                this.RowSpanCreatRow(i, this.dataStore);
            }
            else {
                this.CreatRow(i, this.dataStore);
            }
        }
        document.getElementById(this.id + '_mainLeftDiv').appendChild(this.LmainTable);
        document.getElementById(this.id + '_mainLeftDiv').appendChild(this.LmainTable);
        document.getElementById(this.id + '_mainRightDiv').appendChild(this.RmainTable);
        var divScrol = document.createElement('div');
        divScrol.style.height = '20px';
        document.getElementById(this.id + '_mainLeftDiv').appendChild(divScrol);
        this.frozenTable();
    }
    JSGrid.prototype.frozenTable = function () {
        var id = this.id;
        var rightWidth = document.getElementById(id).clientWidth - document.getElementById(id + '_headLeftTB').offsetWidth;
        var leftHeight = document.getElementById(id).clientHeight - document.getElementById(id + '_headRightTB').offsetHeight;

        document.getElementById(id + '_mainRightDiv').style.width = rightWidth + "px";
        document.getElementById(id + '_mainRightDiv').style.height = leftHeight + "px";

        document.getElementById(id + '_headRightDiv').style.width = rightWidth + "px";
        document.getElementById(id + '_mainLeftDiv').style.height = leftHeight + "px";


        document.getElementById(id + '_mainRightDiv').scrollLeft = 0;
        document.getElementById(id + '_mainRightDiv').scrollTop = 0;
        document.getElementById(id + '_headRightDiv').scrollLeft = document.getElementById(id + '_mainRightDiv').scrollLeft;

        document.getElementById(id + '_mainRightDiv').onscroll = function () {
            document.getElementById(id + '_headRightDiv').scrollLeft = document.getElementById(id + '_mainRightDiv').scrollLeft;
            document.getElementById(id + '_mainLeftDiv').scrollTop = document.getElementById(id + '_mainRightDiv').scrollTop;

        };

    }

    JSGrid.prototype.expend = function (SpanRowid) {
        var obj = document.getElementById(SpanRowid);
        if (obj.Isexpend == true || obj.Isexpend == null) {
            for (var i = 0; i < test.rowSpanGroupData.length; i++) {
                if (test.rowSpanGroupData[i].id == SpanRowid) {
                    for (var rowid in test.rowSpanGroupData[i].rows) {
                        document.getElementById(test.rowSpanGroupData[i].rows[rowid]).style.display = 'none';
                    }
                }
            }
            obj.Isexpend = false;
        } else {
            for (var i = 0; i < test.rowSpanGroupData.length; i++) {
                if (test.rowSpanGroupData[i].id == SpanRowid) {
                    for (var rowid in test.rowSpanGroupData[i].rows) {
                        document.getElementById(test.rowSpanGroupData[i].rows[rowid]).style.display = '';
                    }
                }
            }
            obj.Isexpend = true;
        }
        return obj.Isexpend;
    }

    exports('jsGrid', JSGrid);
});