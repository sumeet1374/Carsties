﻿using System.Reflection.Metadata.Ecma335;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionsController(AuctionDbContext context, IMapper mapper,IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDTO>>> GetAllActions(string date)
    {
        var query = _context.Auctions.OrderBy(x=>x.Item.Make).AsQueryable();

        if(!string.IsNullOrEmpty(date))
        {
            query.Where(x=>x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDTO>(_mapper.ConfigurationProvider).ToListAsync();
        // var auctions = await _context.Auctions.Include(x => x.Item).OrderBy(x => x.Item.Make).ToListAsync();
        // return _mapper.Map<List<AuctionDTO>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDTO>> GetAuctionsById(Guid id)
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null)
        {
            return NotFound();
        }
        return _mapper.Map<AuctionDTO>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO dto)
    {
        var entity = _mapper.Map<Auction>(dto);
        // To do add user as seller
        entity.Seller = "test"; // Just for now
        _context.Auctions.Add(entity);

         var returnDto = _mapper.Map<AuctionDTO>(entity);
         var newacution = _mapper.Map<AuctionCreated>(returnDto);
         await _publishEndpoint.Publish<AuctionCreated>(newacution);
        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return BadRequest("Could not save changes to the DB");
   
       



        return CreatedAtAction(nameof(GetAuctionsById), new { entity.Id }, returnDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDTO dto)
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null)
            return NotFound();

        // To do to Check Seller Name matches the user name

        auction.Item.Make = dto.Make ?? auction.Item.Make;
        auction.Item.Model = dto.Make ?? auction.Item.Model;
        auction.Item.Color = dto.Make ?? auction.Item.Color;
        auction.Item.Mileage = dto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = dto.Year ?? auction.Item.Year;

        var result = await _context.SaveChangesAsync() > 0;
        if (result)
            return Ok();

        return BadRequest("Problem saving changes.");

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null)
            return NotFound();
        // To do to Check Seller Name matches the user name
        _context.Auctions.Remove(auction);

        var result = await _context.SaveChangesAsync() > 0;
        if (result)
            return Ok();

        return BadRequest("Problem deleting changes.");

    }
}
