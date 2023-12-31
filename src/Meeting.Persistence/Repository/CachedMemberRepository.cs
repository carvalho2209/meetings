﻿using Meeting.Domain.Entities;
using Meeting.Domain.Repositories;
using Meeting.Domain.ValueObjects;
using Meeting.Persistence.Constants;
using Meeting.Persistence.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Meeting.Persistence.Repository;

public class CachedMemberRepository : IMemberRepository
{
    private readonly IMemberRepository _decorated;
    private readonly IDistributedCache _distributedCache;
    private readonly ApplicationDbContext _dbContext;

    public CachedMemberRepository(IMemberRepository decorated, IDistributedCache distributedCache, ApplicationDbContext dbContext)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
        _dbContext = dbContext;
    }

    public Task<List<Member?>> GetAllMembers(CancellationToken cancellationToken = default) =>
        _decorated.GetAllMembers(cancellationToken)!;

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        //string key = $"member-{id}";

        string? cachedMember = await _distributedCache.GetStringAsync(
            CacheKeys.MemberById(id),
            cancellationToken);

        Member? member;
        if (string.IsNullOrEmpty(cachedMember))
        {
            member = await _decorated.GetByIdAsync(id, cancellationToken);

            if (member is null)
            {
                return member;
            }

            await _distributedCache.SetStringAsync(
                CacheKeys.MemberById(id),
                JsonConvert.SerializeObject(member),
                cancellationToken);

            return member;
        }

        member = JsonConvert.DeserializeObject<Member?>(
            cachedMember,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });

        if (member is not null)
        {
            _dbContext.Set<Member>().Attach(member);
        }

        return member;
    }

    public async Task<Member?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        string? cachedMember = await _distributedCache.GetStringAsync(
            CacheKeys.MemberByEmail(email),
            cancellationToken);

        Member? member;
        if (string.IsNullOrEmpty(cachedMember))
        {
            member = await _decorated.GetByEmailAsync(email, cancellationToken);

            if (member is null)
            {
                return member;
            }

            await _distributedCache.SetStringAsync(
                CacheKeys.MemberByEmail(email),
                JsonConvert.SerializeObject(member),
                cancellationToken);

            return member;
        }

        member = JsonConvert.DeserializeObject<Member?>(
            cachedMember,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });

        if (member is not null)
        {
            _dbContext.Set<Member>().Attach(member);
        }

        return member;
    }

    public Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
        => _decorated.IsEmailUniqueAsync(email, cancellationToken);

    public void Add(Member member) => _decorated.Add(member);

    public void Update(Member member) => _decorated.Update(member);
}
