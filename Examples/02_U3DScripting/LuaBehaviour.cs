/*
 * Modified by Omid Saadat
 * 
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

[System.Serializable]
public class Injection
{
    public string name;
    public GameObject value;
}

[LuaCallCSharp]
public class LuaBehaviour : MonoBehaviour
{
    public LuaScript luaScript;
    public Injection[] injections;

    internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 
    private LuaTable scriptEnv;

    private Action luaStart;
    private Action luaOnEnable;
    private Action luaOnDisable;
    private Action luaUpdate;
    private Action luaOnDestroy;
    private Action<bool> luaOnApplicationFocus;
    private Action<bool> luaOnApplicationPause;
    private Action luaOnApplicationQuit;
    private Action<Collision> luaOnCollisionEnter;
    private Action<Collision2D> luaOnCollisionEnter2D;
    private Action<Collision> luaOnCollisionExit;
    private Action<Collision2D> luaOnCollisionExit2D;
    private Action<Collision> luaOnCollisionStay;
    private Action<Collision2D> luaOnCollisionStay2D;
    private Action luaOnMouseDown;
    private Action luaOnMouseDrag;
    private Action luaOnMouseEnter;
    private Action luaOnMouseExit;
    private Action luaOnMouseOver;
    private Action luaOnMouseUp;
    private Action luaOnMouseUpAsButton;
    private Action<Collider> luaOnTriggerEnter;
    private Action<Collider2D> luaOnTriggerEnter2D;
    private Action<Collider> luaOnTriggerExit;
    private Action<Collider2D> luaOnTriggerExit2D;
    private Action<Collider> luaOnTriggerStay;
    private Action<Collider2D> luaOnTriggerStay2D;

    void Awake()
    {
        scriptEnv = luaEnv.NewTable();

        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("self", this);
        foreach (var injection in injections)
        {
            scriptEnv.Set(injection.name, injection.value);
        }

        luaEnv.DoString(luaScript.text, "LuaBehaviour", scriptEnv);

        Action luaAwake = scriptEnv.Get<Action>("awake");
        scriptEnv.Get("start", out luaStart);
        scriptEnv.Get("update", out luaUpdate);
        scriptEnv.Get("ondestroy", out luaOnDestroy);
        scriptEnv.Get("onEnable", out luaOnEnable);
        scriptEnv.Get("onDisable", out luaOnDisable);
        scriptEnv.Get("onApplicationFocus", out luaOnApplicationFocus);
        scriptEnv.Get("onApplicationPause", out luaOnApplicationPause);
        scriptEnv.Get("onApplicationQuit", out luaOnApplicationQuit);
        scriptEnv.Get("onCollisionEnter", out luaOnCollisionEnter);
        scriptEnv.Get("onCollisionEnter2D", out luaOnCollisionEnter2D);
        scriptEnv.Get("onCollisionExit", out luaOnCollisionExit);
        scriptEnv.Get("onCollisionExit2D", out luaOnCollisionExit2D);
        scriptEnv.Get("onCollisionStay", out luaOnCollisionStay);
        scriptEnv.Get("onCollisionStay2D", out luaOnCollisionStay2D);
        scriptEnv.Get("onMouseDown", out luaOnMouseDown);
        scriptEnv.Get("onMouseDrag", out luaOnMouseDrag);
        scriptEnv.Get("onMouseEnter", out luaOnMouseEnter);
        scriptEnv.Get("onMouseExit", out luaOnMouseExit);
        scriptEnv.Get("onMouseOver", out luaOnMouseOver);
        scriptEnv.Get("onMouseUp", out luaOnMouseUp);
        scriptEnv.Get("onMouseUpAsButton", out luaOnMouseUpAsButton);
        scriptEnv.Get("onTriggerEnter", out luaOnTriggerEnter);
        scriptEnv.Get("onTriggerEnter2D", out luaOnTriggerEnter2D);
        scriptEnv.Get("onTriggerExit", out luaOnTriggerExit);
        scriptEnv.Get("onTriggerExit2D", out luaOnTriggerExit2D);
        scriptEnv.Get("onTriggerStay", out luaOnTriggerStay);
        scriptEnv.Get("onTriggerStay2D", out luaOnTriggerStay2D);

        if (luaAwake != null)
        {
            luaAwake();
        }
    }


    // Use this for initialization
    void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    private void OnEnable()
    {
        if (luaOnEnable != null) luaOnEnable();
    }

    private void OnDisable()
    {
        if (luaOnDisable != null) luaOnDisable();
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (luaOnApplicationFocus != null) luaOnApplicationFocus.Invoke(focusStatus);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (luaOnApplicationPause != null) luaOnApplicationPause.Invoke(pauseStatus);
    }

    private void OnApplicationQuit()
    {
        if (luaOnApplicationQuit != null) luaOnApplicationQuit.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (luaOnCollisionEnter != null) luaOnCollisionEnter.Invoke(other);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (luaOnCollisionEnter2D != null) luaOnCollisionEnter2D.Invoke(other);
    }

    private void OnCollisionExit(Collision other)
    {
        if (luaOnCollisionExit != null) luaOnCollisionExit.Invoke(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (luaOnCollisionExit2D != null) luaOnCollisionExit2D.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        if (luaOnCollisionStay != null) luaOnCollisionStay.Invoke(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (luaOnCollisionStay2D != null) luaOnCollisionStay2D.Invoke(other);
    }

    private void OnMouseDown()
    {
        if (luaOnMouseDown != null) luaOnMouseDown.Invoke();
    }

    private void OnMouseDrag()
    {
        if (luaOnMouseDrag != null) luaOnMouseDrag.Invoke();
    }

    private void OnMouseEnter()
    {
        if (luaOnMouseEnter != null) luaOnMouseEnter.Invoke();
    }

    private void OnMouseExit()
    {
        if (luaOnMouseExit != null) luaOnMouseExit.Invoke();
    }

    private void OnMouseOver()
    {
        if (luaOnMouseOver != null) luaOnMouseOver.Invoke();
    }

    private void OnMouseUp()
    {
        if (luaOnMouseUp != null) luaOnMouseUp.Invoke();
    }

    private void OnMouseUpAsButton()
    {
        if (luaOnMouseUpAsButton != null) luaOnMouseUpAsButton.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (luaOnTriggerEnter != null) luaOnTriggerEnter.Invoke(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (luaOnTriggerEnter2D != null) luaOnTriggerEnter2D.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (luaOnTriggerExit != null) luaOnTriggerExit.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (luaOnTriggerExit2D != null) luaOnTriggerExit2D.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (luaOnTriggerStay != null) luaOnTriggerStay.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (luaOnTriggerStay2D != null) luaOnTriggerStay2D.Invoke(other);
    }

    // Update is called once per frame
    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
        if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            LuaBehaviour.lastGCTime = Time.time;
        }
    }

    void OnDestroy()
    {
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
        scriptEnv.Dispose();
        injections = null;
    }
}
