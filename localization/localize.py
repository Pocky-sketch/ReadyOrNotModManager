#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Ready Or Not Mod Manager 简体中文汉化脚本 (by Pocky-sketch)

用法:
    python localize.py <源码根目录>

规则:
  * XAML: 只替换 Text/Content/Header/ToolTip/Title 等**展示属性**的值，绝不触碰
    x:Name / Tag / Click / Style / Binding / x:Key 等标识与逻辑属性。
  * .cs: 只替换 strings.zh.json 中登记的**用户可见字面量**。
  * 路径、配置键名 (settings.json 字段)、主题内部 key (tactical/dark/...)、
    状态枚举值 (Deployed/Queued/...)、URL、品牌名一律保留英文原样。
"""
import json, os, re, sys

HERE = os.path.dirname(os.path.abspath(__file__))

def load(name):
    with open(os.path.join(HERE, name), encoding="utf-8") as fh:
        return json.load(fh)

SHOW_ATTRS = ("Text", "Content", "Header", "ToolTip", "Title")

def localize_xaml(path, table):
    with open(path, encoding="utf-8") as fh:
        text = fh.read()
    hits = 0
    for en, zh in table.items():
        for attr in SHOW_ATTRS:
            needle = f'{attr}="{en}"'
            repl = f'{attr}="{zh}"'
            if needle in text:
                hits += text.count(needle)
                text = text.replace(needle, repl)
    with open(path, "w", encoding="utf-8", newline="") as fh:
        fh.write(text)
    return hits

def localize_cs(path, table):
    with open(path, encoding="utf-8") as fh:
        text = fh.read()
    hits = 0
    for en, zh in table.items():
        needle = '"%s"' % en
        if needle in text:
            hits += text.count(needle)
            text = text.replace(needle, '"%s"' % zh)
    with open(path, "w", encoding="utf-8", newline="") as fh:
        fh.write(text)
    return hits

def main():
    root = sys.argv[1] if len(sys.argv) > 1 else os.path.join(HERE, "..")
    xaml_t = load("strings.zh.xaml.json")
    cs_t = load("strings.zh.cs.json")
    app = os.path.join(root, "ReadyOrNotModManager.App")

    total = 0
    for dirpath, _dirs, files in os.walk(app):
        for name in files:
            p = os.path.join(dirpath, name)
            rel = os.path.relpath(p, root)
            if name.endswith(".xaml"):
                n = localize_xaml(p, xaml_t)
            elif name.endswith(".cs"):
                n = localize_cs(p, cs_t)
            else:
                continue
            total += n
            if n:
                print(f"  {rel}: {n} 处")
    print(f"替换合计: {total} 处")

if __name__ == "__main__":
    main()
