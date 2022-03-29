// <copyright file="HttpClientBase.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>11.09.2017</date>
// <summary>Implements the HTTP client base class</summary>
using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TSoft.TVServer.Entities;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Interfaces;
using TSoft.TVServer.Configurations;
using TSoft.TVServer.Helpers;

namespace TSoft.TVServer
{
	/// <summary> A HTTP client base. </summary>
	public abstract class HttpClientBase
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the TSoft.TVServer.HttpClientBase class. </summary>
		/// <param name="logger"> The logger. </param>
		public HttpClientBase(AbstractLogger logger)
		{
			this._Logger = logger;
		}
		#endregion

		#region [Fields]
		/// <summary> The logger. </summary>
		protected readonly AbstractLogger _Logger;

		/// <summary> The serializers. </summary>
		protected readonly ConcurrentDictionary<string, XmlSerializer> _Serializers = new ConcurrentDictionary<string, XmlSerializer>();
		#endregion

		#region [Public properties]
		/// <summary> Gets or sets a value indicating whether the client initialized. </summary>
		/// <value> True if client initialized, false if not. </value>
		public bool ClientInitialized { get; set; }

		/// <summary> Gets or sets the password. </summary>
		/// <value> The password. </value>
		public string Password { get; set; }

		/// <summary> Gets or sets a value indicating whether the requires authentication. </summary>
		/// <value> True if requires authentication, false if not. </value>
		public bool RequiresAuthentication { get; set; }

		/// <summary> Gets or sets URL of the document. </summary>
		/// <value> The URL. </value>
		public string Url { get; set; }

		/// <summary> Gets or sets the name of the user. </summary>
		/// <value> The name of the user. </value>
		public string UserName { get; set; }
		#endregion

		#region [Public methods]
		/// <summary>
		/// Deserialize this TSoft.TVServer.HttpClientBase to the given stream. </summary>
		/// <typeparam name="valueType"> Type of the value type. </typeparam>
		/// <param name="xml"> The XML. </param>
		/// <returns> A valueType. </returns>
		public valueType Deserialize<valueType>(string xml)
		{
			try
			{
				if (!string.IsNullOrEmpty(xml))
				{
					using (var stringReader = new StringReader(xml))
					{
						using (var reader = new XmlTextReader(stringReader))
						{
							reader.Namespaces = false;
							//var serializer = this.GetSerializer(typeof(valueType));
							XmlSerializer serializer = new XmlSerializer(typeof(valueType));
							object serializerDeserialize = serializer.Deserialize(reader);
							return (valueType)serializerDeserialize;
						}
					}
				}
				else
				{
					return default(valueType);
				}
			}
			catch (Exception ex)
			{
				this._Logger.ErrorException("Cannot deserialize Data : {0}", ex, xml);
				return default(valueType);
			}
		}

		/// <summary> Gets response asynchronous. </summary>
		/// <param name="postData"> Information describing the post. </param>
		/// <returns> The asynchronous result that yields the response asynchronous. </returns>
		public async Task<Response> GetResponseAsync(Dictionary<string, string> postData)
		{
			Response response;
			try
			{
				var resultString = await this.HttpPostAsync(postData, CancellationToken.None).ConfigureAwait(false);
				if (!string.IsNullOrEmpty(resultString))
				{
					response = Deserialize<Response>(resultString) as Response;
					return response;
				}
				else
				{
					return new Response();
				}
			}
			catch (Exception e)
			{
				response = new Response { Status = EnumStatusCode.STATUS_INVALID_DATA, Exception = e };
				this._Logger.ErrorException("GetResponse error :{0}", e);
				return response;
			}
		}

		/// <summary> Gets response asynchronous. </summary>
		/// <typeparam name="T"> Generic type parameter. </typeparam>
		/// <param name="request_object"> The request object. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the response asynchronous. </returns>
		public async Task<Response> GetResponseAsync<T>(T request_object, CancellationToken cancellationToken) where T : IRequest
		{
			Response response;
			string xml_string;
			try
			{
				xml_string = Serialize<T>(request_object);
				var command = this.GetHttpHeaderDictionary(request_object.HttpCommand, xml_string);
				var textStream = await this.HttpPostAsync(command, cancellationToken).ConfigureAwait(false);
				var deserialize = Deserialize<Response>(textStream);
				response = deserialize as Response;
				return response;
			}
			catch (Exception e)
			{
				response = new Response { Status = EnumStatusCode.STATUS_INVALID_DATA, Exception = e };
				this._Logger.ErrorException("GetResponse error :{0}", e);
				return response;
			}
		}

		public async Task<ResponseObject<T, R>> GetResponseObjectAsync<T, R>(R request_object, CancellationToken cancellationToken, bool includeResponse = true, bool includeRequest = true)
			where T : class
			where R : class, IRequest
		{
			object responseObject;
			Response response;
			var responseObjectValue = new ResponseObject<T, R>();

			try
			{
				response = await this.GetResponseAsync<R>(request_object, cancellationToken).ConfigureAwait(false);
				if (includeResponse)
				{
					responseObjectValue.Response = response;
				}

				if (includeRequest)
				{
					responseObjectValue.Request = request_object;
				}

				string responseString = this.CheckXmlString(response.Result);
				responseObjectValue.Status = response.Status;

				if (EnumStatusCode.STATUS_OK == (EnumStatusCode)response.Status)
				{
					if (!(request_object is Response))
					{
						responseObject = (object)Deserialize<T>(responseString);
					}
					else
					{
						responseObject = (object)null;
					}
				}
				else
				{
					return responseObjectValue;
				}
			}
			catch (Exception e)
			{
				this._Logger.ErrorException("GetResponseObject error :{0}", e);
				response = new Response { Status = EnumStatusCode.STATUS_INVALID_DATA, Exception = e };
				return responseObjectValue;
			}

			responseObjectValue.ResultObject = (T)responseObject;
			return responseObjectValue;
		}

		/// <summary> HTTP post asynchronous. </summary>
		/// <param name="postParameters"> Options for controlling the post. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields a string. </returns>
		public abstract Task<string> HttpPostAsync(Dictionary<string, string> postParameters, CancellationToken cancellationToken);

		/// <summary>
		/// Serialize this TSoft.TVServer.HttpClientBase to the given stream. </summary>
		/// <typeparam name="TValueType"> Type of the value type. </typeparam>
		/// <param name="data"> The data. </param>
		/// <returns> A string. </returns>
		public string Serialize<TValueType>(TValueType data)
		{
			try
			{
				using (var writer = new StringWriter())
				{
					var ns = new XmlSerializerNamespaces();
					ns.Add("", "");
					var serializer = this.GetSerializer(typeof(TValueType));
					serializer.Serialize(writer, data);

					return writer.ToString();
				}
			}
			catch (Exception ex)
			{
				this._Logger.ErrorException("Cannot serialize Data :", ex);
				return string.Empty;
			}
		}
		#endregion

		#region [Private methods]
		/// <summary> Check XML string. </summary>
		/// <param name="xml"> The XML. </param>
		/// <returns> A string. </returns>
		private string CheckXmlString(string xml)
		{
			string xmlReturn = string.Empty;
			if (string.IsNullOrEmpty(xml))
			{
				return string.Empty;
			}

			var xmlDoc = new XmlDocument();
			try
			{
				//DVBLink 4.6 channel correction
				if (xml.StartsWith("?", StringComparison.Ordinal) || xml.StartsWith("?<", StringComparison.Ordinal) || (xml.Length > 0 && xml[0] == '?'))
				{
					xmlReturn = xml.Remove(0, 1);
					this._Logger.Info(xml);
				}
				else
				{
					xmlReturn = xml;
				}

				xmlDoc.LoadXml(xmlReturn);
			}
			catch (Exception ex)
			{
				this._Logger.ErrorException("XML file error :{0}", ex, xml);
			}
			return xmlReturn;
		}

		/// <summary> Gets HTTP header dictionary. </summary>
		/// <param name="command"> The command. </param>
		/// <param name="param"> The parameter. </param>
		/// <returns> The HTTP header dictionary. </returns>
		private Dictionary<string, string> GetHttpHeaderDictionary(string command, string param)
		{
            var postData = new Dictionary<string, string>
            {
                { "command", command },
                { "xml_param", param }
            };

            return postData;
		}

		/// <summary> Gets a serializer. </summary>
		/// <param name="type"> The type. </param>
		/// <returns> The serializer. </returns>
		private XmlSerializer GetSerializer(Type type)
		{
			var key = type.FullName;
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return this._Serializers.GetOrAdd(key, k => xmlSerializer);
		}
		#endregion

		#region [Nested Class]
		public class NamespaceIgnorantXmlTextReader : XmlTextReader
		{
			#region [Constructors]
			public NamespaceIgnorantXmlTextReader(System.IO.TextReader reader) : base(reader) { }
			#endregion

			#region [Public properties]
			public override string NamespaceURI
			{
				get { return ""; }
			}
			#endregion
		}
		#endregion
	}
}
