import { Static, Type } from '@sinclair/typebox'
import { fastify } from 'fastify'
import { createSession, getSession } from './sessions'

const app = fastify({ logger: true })

const BattlegroundCreate = Type.Object({
    player_id: Type.String(),
})

app.post<{ Body: Static<typeof BattlegroundCreate> }>(
    '/battleground/create',
    { schema: { body: BattlegroundCreate } },
    async (req) => {
        const playerId = req.body.player_id
        var session = createSession(playerId)

        return {
            success: true,
            reason: '',
            session_id: session['id']
        }
    }
)

const BattlegroundJoin = Type.Object({
    player_id: Type.String(),
    session_id: Type.String(),
})

app.post<{ Body: Static<typeof BattlegroundJoin> }>(
    '/battleground/join',
    { schema: { body: BattlegroundJoin } },
    async (req, reply) => {
        const { player_id: playerId, session_id: sessionId } = req.body

        const session = getSession(sessionId)
        if (session === null) {
            reply.code(404)
            return { success: false, reason: 'Session not found' }
        }

        if (session.host === playerId) {
            reply.code(400)
            return { success: false, reason: 'Same player id as host' }
        }

        session.guest = playerId

        return {
            success: true,
            reason: ''
        }
    }
)

const BattlegroundInfoRequest = Type.Object({
    session_id: Type.String(),
})

app.post<{ Body: Static<typeof BattlegroundInfoRequest> }>(
    '/battleground/info',
    { schema: { body: BattlegroundInfoRequest } },
    async ({ body: { session_id: sessionId } }, reply) => {
        const session = getSession(sessionId)
        if (session === null) {
            reply.code(404)
            return { success: false, reason: 'Session not found' }
        }

        const hostPoints = session.logs.filter(
            (s) => s.player_id === session.host
        ).length

        const guestPoints = session.logs.filter(
            (s) => s.player_id === session.guest
        ).length

        return {
            success: true,
            reason: '',
            winner_id: hostPoints > guestPoints ? session.host : session.guest,
            available_seats:
                (session.guest === '' ? 1 : 0) + (session.host === '' ? 1 : 0),
        }
    }
)

const BattlegroundLogRequest = Type.Object({
    from: Type.Number(),
    to: Type.Number(),
    session_id: Type.String(),
})

app.post<{ Body: Static<typeof BattlegroundLogRequest> }>(
    '/battleground/battle_log',
    { schema: { body: BattlegroundLogRequest } },
    async ({ body: { from, to, session_id: sessionId } }, reply) => {
        const session = getSession(sessionId)
        if (session === null) {
            reply.code(404)
            return { success: false, reason: 'Session not found' }
        }

        if (to === -1) {
            to = session.logs.length
        }

        return {
            success: true,
            logs: session.logs.slice(from, to),
        }
    }
)

const BattlegroundRequest = Type.Object({
    session_id: Type.String(),
    player_id: Type.String(),
    pos: Type.Object({ x: Type.Number(), y: Type.Number() }),
})

app.post<{ Body: Static<typeof BattlegroundRequest> }>(
    '/battleground',
    { schema: { body: BattlegroundRequest } },
    async (
        {
            body: {
                session_id: sessionId,
                player_id: playerId,
                pos: { x, y },
            },
        },
        reply
    ) => {
        const session = getSession(sessionId)
        if (session === null) {
            reply.code(404)
            return { success: false, reason: 'Session not found' }
        }

        session.logs.push({
            player_id: playerId,
            pos: { x, y },
        })

        return { success: true, reason: '' }
    }
)

app.post<{ Body: Static<typeof BattlegroundRequest> }>(
    '/battleground/board/upload',
    { schema: { body: BattlegroundRequest } },
    async (
        {
            body: {
                session_id: sessionId,
                player_id: playerId,
                pos: { x, y },
            },
        },
        reply
    ) => {
        const session = getSession(sessionId)
        if (session === null) {
            reply.code(404)
            return { success: false, reason: 'Session not found' }
        }

        session.pos = { x, y }

        return { success: true, reason: '' }
    }
)

app.post<{ Body: Static<typeof BattlegroundRequest> }>(
    '/battleground/board/sync',
    { schema: { body: BattlegroundRequest } },
    async (
        {
            body: {
                session_id: sessionId
            },
        },
        reply
    ) => {
        const session = getSession(sessionId)
        if (session === null) {
            reply.code(404)
            return { success: false, reason: 'Session not found' }
        }

        return { success: true, reason: '', pos: session.pos }
    }
)

app.listen(8000, '0.0.0.0')
