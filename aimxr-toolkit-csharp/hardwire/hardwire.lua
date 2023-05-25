${
	["MoonSharp.Interpreter.Interop.AnonWrapper"] = ${
		skip = true,
	},
	["MoonSharp.Interpreter.Serialization.Json.JsonNull"] = ${
		skip = true,
	},
	["AimXRToolkit.Interactions.Proxies.ProxyButton"] = ${
		visibility = "public",
		class = "MoonSharp.Interpreter.Interop.StandardUserDataDescriptor",
		members = ${
			__new = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "__new",
				decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = ".ctor",
						ctor = true,
						special = true,
						visibility = "public",
						ret = "AimXRToolkit.Interactions.Proxies.ProxyButton",
						decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
						static = true,
						extension = false,
						params = ${
							[1] = ${
								name = "button",
								type = "AimXRToolkit.Interactions.Button",
								origtype = "AimXRToolkit.Interactions.Button",
								default = false,
								out = false,
								ref = false,
								varargs = false,
								restricted = false,
							},
						},
					},
				},
			},
			OnTouch = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "OnTouch",
				decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "OnTouch",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Void",
						decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
			GetType = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "GetType",
				decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "GetType",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Type",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
			ToString = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "ToString",
				decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "ToString",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.String",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
			Equals = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "Equals",
				decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "Equals",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Boolean",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${
							[1] = ${
								name = "obj",
								type = "System.Object",
								origtype = "System.Object",
								default = false,
								out = false,
								ref = false,
								varargs = false,
								restricted = false,
							},
						},
					},
				},
			},
			GetHashCode = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "GetHashCode",
				decltype = "AimXRToolkit.Interactions.Proxies.ProxyButton",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "GetHashCode",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Int32",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
		},
		metamembers = ${ },
	},
}