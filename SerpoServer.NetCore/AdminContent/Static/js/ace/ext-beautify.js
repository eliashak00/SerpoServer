define("ace/ext/beautify",
    ["require", "exports", "module", "ace/token_iterator"],
    function(e, t, n) {
        "use strict";

        function i(e, t) { return e.type.lastIndexOf(t + ".xml") > -1 }

        var r = e("../token_iterator").TokenIterator;
        t.singletonTags =
        [
            "area", "base", "br", "col", "command", "embed", "hr", "html", "img", "input", "keygen", "link", "meta",
            "param", "source", "track", "wbr"
        ], t.blockTags =
        [
            "article", "aside", "blockquote", "body", "div", "dl", "fieldset", "footer", "form", "head", "header",
            "html", "nav", "ol", "p", "script", "section", "style", "table", "tbody", "tfoot", "thead", "ul"
        ], t.beautify = function(e) {
            var n = new r(e, 0, 0),
                s = n.getCurrentToken(),
                o = e.getTabString(),
                u = t.singletonTags,
                a = t.blockTags,
                f,
                l = !1,
                c = !1,
                h = !1,
                p = "",
                d = "",
                v = "",
                m = 0,
                g = 0,
                y = 0,
                b = 0,
                w = 0,
                E = 0,
                S = !1,
                x,
                T = 0,
                N = 0,
                C = [],
                k = !1,
                L,
                A = !1,
                O = !1,
                M = !1,
                _ = !1,
                D = { 0: 0 },
                P = {},
                H = function() { f && f.value && f.type !== "string.regexp" && (f.value = f.value.trim()) },
                B = function() { p = p.replace(/ +$/, "") },
                j = function() { p = p.trimRight(), l = !1 };
            while (s !== null) {
                T = n.getCurrentTokenRow(), C = n.$rowTokens, f = n.stepForward();
                if (typeof s != "undefined") {
                    d = s.value, w = 0, M = v === "style" || e.$modeId === "ace/mode/css", i(s, "tag-open")
                        ? (O = !0, f && (_ = a.indexOf(f.value) !== -1), d === "</" &&
                            (_ && !l && N < 1 && N++, M && (N = 1), w = 1, _ = !1))
                        : i(s, "tag-close")
                        ? O = !1
                        : i(s, "comment.start")
                        ? _ = !0
                        : i(s, "comment.end") && (_ = !1), !O &&
                        !N &&
                        s.type === "paren.rparen" &&
                        s.value.substr(0, 1) === "}" &&
                        N++, T !== x && (N = T, x && (N -= x));
                    if (N) {
                        j();
                        for (; N > 0; N--) p += "\n";
                        l = !0, !i(s, "comment") && !s.type.match(/^(comment|string)$/) && (d = d.trimLeft())
                    }
                    if (d) {
                        s.type === "keyword" && d.match(/^(if|else|elseif|for|foreach|while|switch)$/)
                            ? (P[m] = d, H(), h =
                                !0, d.match(/^(else|elseif)$/) && p.match(/\}[\s]*$/) && (j(), c = !0))
                            : s.type === "paren.lparen"
                            ? (H(), d.substr(-1) === "{" && (h = !0, A = !1, O || (N = 1)),
                                d.substr(0, 1) === "{" &&
                                (c = !0, p.substr(-1) !== "[" && p.trimRight().substr(-1) === "["
                                    ? (j(), c = !1)
                                    : p.trimRight().substr(-1) === ")"
                                    ? j()
                                    : B()))
                            : s.type === "paren.rparen"
                            ? (w =
                                    1, d.substr(0, 1) === "}" &&
                                (P[m - 1] === "case" && w++, p.trimRight().substr(-1) === "{"
                                    ? j()
                                    : (c = !0, M && (N += 2))),
                                d.substr(0, 1) === "]" &&
                                    p.substr(-1) !== "}" &&
                                    p.trimRight().substr(-1) === "}" &&
                                    (c = !1, b++, j()),
                                d.substr(0, 1) === ")" &&
                                    p.substr(-1) !== "(" &&
                                    p.trimRight().substr(-1) === "(" &&
                                    (c = !1, b++, j()), B())
                            : s.type !== "keyword.operator" && s.type !== "keyword" ||
                            !d.match(/^(=|==|===|!=|!==|&&|\|\||and|or|xor|\+=|.=|>|>=|<|<=|=>)$/)
                            ? s.type === "punctuation.operator" && d === ";"
                            ? (j(), H(), h = !0, M && N++)
                            : s.type === "punctuation.operator" && d.match(/^(:|,)$/)
                            ? (j(), H(), h = !0, l = !1)
                            : s.type === "support.php_tag" && d === "?>" && !l
                            ? (j(), c = !0)
                            : i(s, "attribute-name") && p.substr(-1).match(/^\s$/)
                            ? c = !0
                            : i(s, "attribute-equals")
                            ? (B(), H())
                            : i(s, "tag-close") && (B(), d === "/>" && (c = !0))
                            : (j(), H(), c = !0, h = !0);
                        if (l &&
                            (!s.type.match(/^(comment)$/) || !!d.substr(0, 1).match(/^[/#]$/)) &&
                            (!s.type.match(/^(string)$/) || !!d.substr(0, 1).match(/^['"]$/))) {
                            b = y;
                            if (m > g) {
                                b++;
                                for (L = m; L > g; L--) D[L] = b
                            } else m < g && (b = D[m]);
                            g = m, y = b, w && (b -= w), A && !E && (b++, A = !1);
                            for (L = 0; L < b; L++) p += o
                        }
                        s.type === "keyword" && d.match(/^(case|default)$/) && (P[m] = d, m++), s.type === "keyword" &&
                                d.match(/^(break)$/) &&
                                P[m - 1] &&
                                P[m - 1].match(/^(case|default)$/) &&
                                m--, s.type === "paren.lparen" && (E += (d.match(/\(/g) || []).length, m += d.length),
                            s.type === "keyword" && d.match(/^(if|else|elseif|for|while)$/)
                                ? (A = !0, E = 0)
                                : !E && d.trim() && s.type !== "comment" && (A = !1);
                        if (s.type === "paren.rparen") {
                            E -= (d.match(/\)/g) || []).length;
                            for (L = 0; L < d.length; L++) m--, d.substr(L, 1) === "}" && P[m] === "case" && m--
                        }
                        c && !l && (B(), p.substr(-1) !== "\n" && (p += " ")), p += d, h && (p += " "), l = !1, c =
                            !1, h = !1;
                        if (i(s, "tag-close") && (_ || a.indexOf(v) !== -1) || i(s, "doctype") && d === ">")
                            _ && f && f.value === "</" ? N = -1 : N = 1;
                        i(s, "tag-open") && d === "</"
                            ? m--
                            : i(s, "tag-open") && d === "<" && u.indexOf(f.value) === -1
                            ? m++
                            : i(s, "tag-name")
                            ? v = d
                            : i(s, "tag-close") && d === "/>" && u.indexOf(v) === -1 && m--, x = T
                    }
                }
                s = f
            }
            p = p.trim(), e.doc.setValue(p)
        }, t.commands = [{ name: "beautify", exec: function(e) { t.beautify(e.session) }, bindKey: "Ctrl-Shift-B" }]
    });
(function() {
    window.require(["ace/ext/beautify"],
        function(m) {
            if (typeof module == "object" && typeof exports == "object" && module) {
                module.exports = m;
            }
        });
})();