﻿// <copyright file="ArgsHelper.cs" company="ted-sharp">
// Copyright (c) ted-sharp. All rights reserved.
// </copyright>

namespace Aloe.Utils.CommandLine;

/// <summary>
/// コマンドライン引数の前処理を行うユーティリティクラス。
/// </summary>
public static class ArgsHelper
{
    /// <summary>
    /// コマンドライン引数を前処理します。
    /// オプションが単独('--IsFlag')で bool を表す場合、値が指定されていなければ自動的に "true" を補完します。('--IsFlag' → '--IsFlag true')
    /// 短いオプション('-a')で、オプションと値が連結している場合は分割します。('-avalue' → '-a value')
    /// </summary>
    /// <remarks>
    /// `ConfigurationBuilder` ではセパレータ―(`=`) に対応しているので、前処理では分割しません。
    /// 処理の優先順位：
    /// 1. フラグオプションの処理（--IsFlag → --IsFlag true）
    /// 2. 短いオプションの処理（-avalue → -a value）
    /// 3. その他の引数はそのまま保持
    /// </remarks>
    /// <param name="args">処理対象のコマンドライン引数</param>
    /// <param name="flagArgs">フラグとして扱うオプションのリスト（例：--IsFlag）</param>
    /// <param name="shortArgs">短いオプションのリスト（例：-u, -p）</param>
    /// <returns>前処理されたコマンドライン引数を返します。</returns>
    /// <exception cref="ArgumentNullException">引数がnullの場合にスローされます。</exception>
    public static string[] PreprocessArgs(
        string[] args,
        IEnumerable<string> flagArgs,
        IEnumerable<string> shortArgs)
    {
        ArgumentNullException.ThrowIfNull(args, nameof(args));
        ArgumentNullException.ThrowIfNull(flagArgs, nameof(flagArgs));
        ArgumentNullException.ThrowIfNull(shortArgs, nameof(shortArgs));

        var processedArgs = new List<string>();

        // HashSetを使用して検索パフォーマンスを最適化
        var flags = new HashSet<string>(flagArgs);
        var shorts = new HashSet<string>(shortArgs);

        for (var i = 0; i < args.Length; i++)
        {
            var currentArg = args[i];

            // フラグオプションの処理
            if (flags.Contains(currentArg))
            {
                processedArgs.Add(currentArg);

                // 次のトークンが存在しない、または '-' で始まるなら "true" を付与
                if (i + 1 >= args.Length || args[i + 1].StartsWith("-"))
                {
                    processedArgs.Add("true");
                }

                continue;
            }

            // 短いオプションの処理
            if (TryHandleShortOption(currentArg, shorts, processedArgs))
            {
                continue;
            }

            // 上記に該当しない場合はそのまま追加
            processedArgs.Add(currentArg);
        }

        return processedArgs.ToArray();
    }

    /// <summary>
    /// 短いオプションの処理を行います。
    /// </summary>
    /// <remarks>
    /// 短いオプションの形式：
    /// - 単独のオプション（例：-u）
    /// - オプションと値が連結（例：-uadmin）
    ///
    /// 処理の流れ：
    /// 1. 基本的な形式チェック（'-'で始まり、長さが2以上）
    /// 2. 定義された短いオプションとの照合
    /// 3. オプションと値の分割（必要な場合）
    /// </remarks>
    /// <param name="arg">処理対象の引数</param>
    /// <param name="shortOptions">短いオプションのセット</param>
    /// <param name="processedArgs">処理済みの引数リスト</param>
    /// <returns>短いオプションとして処理された場合はtrue、それ以外はfalse</returns>
    private static bool TryHandleShortOption(
        string arg,
        HashSet<string> shortOptions,
        List<string> processedArgs)
    {
        // 基本的な形式チェック：'-'で始まり、長さが2以上であることを確認
        if (arg.Length <= 1 || !arg.StartsWith("-"))
        {
            return false;
        }

        foreach (var shortOpt in shortOptions)
        {
            if (arg.StartsWith(shortOpt))
            {
                // 短いオプションそのものだけの場合はそのまま追加
                if (arg.Length == shortOpt.Length)
                {
                    processedArgs.Add(arg);
                }
                else
                {
                    // オプションと値が連結されている場合
                    // 例: "-uadmin" → "-u" "admin"
                    // 例: "-ppwd" → "-p" "pwd"
                    processedArgs.Add(shortOpt);
                    processedArgs.Add(arg.Substring(shortOpt.Length));
                }

                return true;
            }
        }

        return false;
    }
}
