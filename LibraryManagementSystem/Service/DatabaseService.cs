﻿using LibraryManagementSystem.DAO;
using LibraryManagementSystem.Entity;
using LibraryManagementSystem.LogSys;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Service
{

    public static class DatabaseService<T> where T : BasicEntity
    {
        public static readonly string LogName = "DatabaseService";
        public static int Create(params T[] elements)
        {
            FreeSql.IInsert<T> insert;
            int rows;
            try
            {
                 insert = Sql.Instance.Insert(elements);
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为{Elements}创建插入对象失败",
                                    LogName, elements);
                throw;
            }
            try
            {
                rows= insert.ExecuteAffrows();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行查询操作{InsertSql}失败",
                                    LogName, insert.ToSql());
                throw;
            }
            return rows;
        }
        public static int Delete(params T[] elements)
        {
            FreeSql.IDelete<T> delete;
            int rows;
            try
            {
                delete = Sql.Instance.Delete<T>(elements);
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为{Elements}创建删除对象失败",
                                    LogName, elements);
                throw;
            }
            try
            {
                rows = delete.ExecuteAffrows();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行删除操作{DeleteSql}失败",
                                    LogName, delete.ToSql());
                throw;
            }
            return rows;
        }
        public static int Update(string[] columns, params T[] elements)
        {
            if (Sql.Instance.Ado.TransactionCurrentThread == null)
                LoggerHolder.Instance.Warning("{LogName}: 未在事务中时更新对象",
                                    LogName);
            FreeSql.IUpdate<T> update;
            int rows;
            try
            {
                update = Sql.Instance.Update<T>()
                .SetSource(new List<T>(elements))
                .UpdateColumns(columns);
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为{Elements}的{Columns}列创建更新对象失败",
                                    LogName, elements, columns);
                throw;
            }
            try
            {
                rows = update.ExecuteAffrows();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行更新操作{UpdateSql}失败",
                                    LogName, update.ToSql());
                throw;
            }
            return rows;
        }
        public static List<T> Query(string condition, int pageIndex, int pageSize, out long count)
        {
            FreeSql.ISelect<T> select;
            List<T> elements;
            try
            {
                select = Sql.Instance.Select<T>()
                .Where(condition)
                .Count(out count)
                .Page(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为类型{T}以条件{Condition}创建查询对象失败",
                                    LogName,typeof(T),condition);
                throw;
            }
            try
            {
                elements=select.ToList();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行查询操作{SelectSql}失败",
                                    LogName, select.ToSql());
                throw;
            }
            return elements;
        }
        public static List<T> Query(string condition)
        {
            FreeSql.ISelect<T> select;
            List<T> elements;
            try
            {
                select = Sql.Instance.Select<T>()
                .Where(condition);
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为类型{T}以条件{Condition}创建查询对象失败",
                                    LogName, typeof(T), condition);
                throw;
            }
            try
            {
                elements = select.ToList();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行查询操作{SelectSql}失败",
                                    LogName, select.ToSql());
                throw;
            }
            return elements;
        }
        public static List<T> Query(params T[] elements)
        {
            FreeSql.ISelect<T> select;
            List<T> refreshedElements;
            try
            {
                select = Sql.Instance.Select<T>(elements);
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为类型{T}创建查询对象失败",
                                    LogName, typeof(T));
                throw;
            }
            try
            {
                refreshedElements = select.ToList();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行查询操作{SelectSql}失败",
                                    LogName, select.ToSql());
                throw;
            }
            return refreshedElements;
        }
        public static void ForUpdate(params T[] elements)
        {
            FreeSql.ISelect<T> forUpdate;
            try
            {
                forUpdate = Sql.Instance.Select<T>(elements)
                    .ForUpdate();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为类型{T}创建悲观锁对象失败",
                                    LogName, typeof(T));
                throw;
            }
            try
            {
                forUpdate.ToOne();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行查询操作{ForUpdateSql}失败",
                                    LogName, forUpdate.ToSql());
                throw;
            }
        }
        public static void ForUpdate(string condition)
        {
            FreeSql.ISelect<T> forUpdate;
            try
            {
                forUpdate = Sql.Instance.Select<T>()
                .Where(condition)
                .ForUpdate();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 为类型{T}以条件{Condition}创建悲观锁对象失败",
                                    LogName, typeof(T), condition);
                throw;
            }
            try
            {
                forUpdate.ToOne();
            }
            catch (Exception ex)
            {
                LoggerHolder.Instance.Error(ex, "{LogName}: 执行查询操作{ForUpdateSql}失败",
                                    LogName, forUpdate.ToSql());
                throw;
            }
        }

    }

}
