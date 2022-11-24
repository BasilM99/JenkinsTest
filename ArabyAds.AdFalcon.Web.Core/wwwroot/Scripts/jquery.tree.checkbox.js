(function ($) {
    $.extend($.tree.plugins, {
        "checkbox": {
            defaults: {
                three_state: true
            },
            get_checkedOrUndeterminded: function (t) {
                if (!t) t = $.tree.focused();
                return t.container.find("a.checked,a.undetermined").parent();
            },
            get_checked: function (t) {
                if (!t) t = $.tree.focused();
                return t.container.find("a.checked").parent();
            },
            get_undeterminded: function (t) {
                if (!t) t = $.tree.focused();
                return t.container.find("a.undetermined").parent();
            },
            get_unchecked: function (t) {
                if (!t) t = $.tree.focused();
                return t.container.find("a:not(.checked, .undetermined)").parent();
            },
            fireEvent: function (container, item, isChecked) {
                if (typeof (treeHnadler) != "undefined") {
                    treeHnadler(container, item, isChecked);
                }
            },
            check: function (n) {
                if (!n) return false;
                var t = $.tree.reference(n);
                n = t.get_node(n);
                var container = t; //t.container;
                if (n.children("a").hasClass("locked")) return false;
                if (n.children("a").hasClass("checked")) return true;
               
                var opts = $.extend(true, {}, $.tree.plugins.checkbox.defaults, t.settings.plugins.checkbox);
                if (opts.three_state) {
                    if (!n.find("li").andSelf().children("a").hasClass("locked")) {
                        n.find("li").andSelf().children("a").removeClass("unchecked undetermined").addClass("checked");
                        n.find("li").andSelf().children("a").each(function(){
                           
                            t.open_branch(this);
                             
                        });
                    }
                   
                    n.parents("li").each(function () {
                        if ($(this).children("ul").find("a:not(.checked):eq(0)").length > 0) {
                            
                            $(this).parents("li").andSelf().children("a").removeClass("unchecked checked").addClass("undetermined");
                            $(this).parents("li").andSelf().children("a").each(function () {
                              
                                t.open_branch(this);

                            });
                            //t.open_all(this);
                            return false;
                            
                        }
                        else {
                            if (!$(this).children("a").hasClass("locked"))
                            {
                                $(this).children("a").removeClass("unchecked undetermined").addClass("checked");


                                $(this).children("a").each(function(){
                             
                                  
                                    t.open_branch(this);
                             
                                });

                                t.open_branch(this);
                            }
                            else
                                return false;
                        }

                      
                    });
                   
                    //t.open_all(this);
                }
                else {
                    if (!n.children("a").hasClass("locked")) {
                        n.children("a").removeClass("unchecked").addClass("checked");
                        n.children("a").each(function(){
                             
                                
                            t.open_branch(this);
                             
                        });

                        t.open_branch(n);
                    
                    }
                    else
                        return false;
                }
                if (container) {
                    $.tree.plugins.checkbox.fireEvent(container, n.children("a").text(), true);
                }
                return true;
            },
            uncheck: function (n) {
                if (!n) return false;
                var t = $.tree.reference(n);
                n = t.get_node(n);
                var container = t; //t.container;
                if (n.children("a").hasClass("locked")) return false;
                if (n.children("a").hasClass("unchecked")) return true;

                var opts = $.extend(true, {}, $.tree.plugins.checkbox.defaults, t.settings.plugins.checkbox);
                if (opts.three_state) {
                    n.find("li").andSelf().children("a").removeClass("checked undetermined").addClass("unchecked");
                    n.parents("li").each(function () {
                        if ($(this).find("a.checked, a.undetermined").length - 1 > 0) {
                            $(this).parents("li").andSelf().children("a").removeClass("unchecked checked").addClass("undetermined");
                            return false;
                        }
                        else $(this).children("a").removeClass("checked undetermined").addClass("unchecked");
                    });
                }
                else n.children("a").removeClass("checked").addClass("unchecked");
                if (container) {
                    $.tree.plugins.checkbox.fireEvent(container, n.children("a").text(), false);
                }
                return true;
            },
            toggle: function (n, container) {
                if (!n) return false;
                var t = $.tree.reference(n);
                n = t.get_node(n);
                if (n.children("a").hasClass("checked")) $.tree.plugins.checkbox.uncheck(n, container);
                else $.tree.plugins.checkbox.check(n);
            },

            callbacks: {
                onchange: function (n, t) {
                    $.tree.plugins.checkbox.toggle(n);
                }
            }
        }
    });
})(jQuery);