﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using Cherry.Interfaces;
using Cherry.Models;
using HMUI;
using IPA.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cherry.UI
{
    internal class RequestDetailView
    {
        internal static readonly FieldAccessor<ImageView, float>.Accessor ImageSkew = FieldAccessor<ImageView, float>.GetAccessor("_skew");

        [UIComponent("cover-image")]
        protected readonly Image _coverImage = null!;

        [UIComponent("requester-button")]
        protected readonly Button _requesterButton = null!;

        [UIComponent("suggestions-button")]
        protected readonly Button _suggestionsButton = null!;

        [UIComponent("title-text")]
        protected readonly CurvedTextMeshPro _titleText = null!;

        [UIComponent("uploader-text")]
        protected readonly CurvedTextMeshPro _uploaderText = null!;

        [UIComponent("requester-text")]
        protected readonly CurvedTextMeshPro _requesterText = null!;

        [UIComponent("rating-text")]
        protected readonly CurvedTextMeshPro _ratingText = null!;

        [UIComponent("time-text")]
        protected readonly CurvedTextMeshPro _timeText = null!;

        [UIComponent("content-root")]
        protected readonly RectTransform _contentRoot = null!;

        [UIComponent("loading-root")]
        protected readonly RectTransform _loadingRoot = null!;

        [UIComponent("song-name-text")]
        protected readonly CurvedTextMeshPro _songNameText = null!;

        [UIComponent("ban-song-button")]
        protected readonly Button _banSongButton = null!;

        [UIComponent("username-text")]
        protected readonly CurvedTextMeshPro _usernameText = null!;

        [UIComponent("ban-requester-session-button")]
        protected readonly Button _banRequesterSessionButton = null!;

        [UIComponent("ban-requester-forever-button")]
        protected readonly Button _banRequesterForeverButton = null!;

        [UIParams]
        protected readonly BSMLParserParams parserParams = null!;

        public event Action<RequestEventArgs>? BanSongButtonClicked;
        public event Action<IRequester>? BanSessionButtonClicked;
        public event Action<IRequester>? BanForeverButtonClicked;
        private RequestEventArgs? _lastRequest;

        [UIAction("ban-song-clicked")] protected void BanSong()
        {
            parserParams.EmitEvent("hide-request");
            BanSongButtonClicked?.Invoke(_lastRequest!);
        }

        [UIAction("ban-session-clicked")] protected void BanSession()
        {
            parserParams?.EmitEvent("hide-request");
            BanSessionButtonClicked?.Invoke(_lastRequest!.Requester);
        }

        [UIAction("ban-forever-clicked")] protected void BanForever()
        {
            parserParams?.EmitEvent("hide-request");
            BanForeverButtonClicked?.Invoke(_lastRequest!.Requester);
        }


        [UIAction("#post-parse")]
        protected void Parsed()
        {
            _coverImage.material = Utilities.UINoGlowRoundEdge;

            _banSongButton.SetSkew(0f);
            _requesterButton.SetSkew(0f);
            _suggestionsButton.SetSkew(0f);
            _banRequesterSessionButton.SetSkew(0f);
            _banRequesterForeverButton.SetSkew(0f);

            _titleText.fontSizeMin = 3f;
            _titleText.fontSizeMax = 4.5f;

            _uploaderText.fontSizeMin = 1.5f;
            _uploaderText.fontSizeMax = 3f;

            _requesterText.fontSizeMin = 1.5f;
            _requesterText.fontSizeMax = 3f;

            _songNameText.fontSizeMin = 1.5f;
            _songNameText.fontSizeMax = 6.5f;

            _usernameText.fontSizeMin = 1.5f;
            _usernameText.fontSizeMax = 6.5f;

            _titleText.enableAutoSizing = true;
            _uploaderText.enableAutoSizing = true;
            _requesterText.enableAutoSizing = true;
            _songNameText.enableAutoSizing = true;
            _usernameText.enableAutoSizing = true;
        }

        public void SetData(string songName, string uploaderName, RequestEventArgs request, Sprite imageCover, float rating)
        {
            _lastRequest = request;
            _contentRoot.gameObject.SetActive(true);
            _loadingRoot.gameObject.SetActive(false);

            _titleText.text = songName;
            _uploaderText.text = uploaderName;
            _requesterText.text = $"requested by <color=#919191>{request.Requester.Username}</color>";
            _coverImage.sprite = imageCover;

            _ratingText.text = string.Format("{0:0%}", rating);
            _ratingText.color = Utilities.Evaluate(rating);
            _timeText.text = request.RequestTime.ToString("h:mm tt");

            _suggestionsButton.interactable = false;

            _songNameText.text = songName;
            _usernameText.text = request.Requester.Username;
        }

        public void SetLoading()
        {
            _contentRoot.gameObject.SetActive(false);
            _loadingRoot.gameObject.SetActive(true);
        }
    }
}