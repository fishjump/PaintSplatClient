import { v4 as uuidv4 } from 'uuid'

interface Session {
    host: string
    guest: string
    id: string
    pos: { x: number, y: number }
    logs: { player_id: string; pos: { x: number; y: number } }[]
}

const globalSessions: Record<string, Session> = {}

export const createSession = (hostPlayer: string): Session => {
    const session = {
        host: hostPlayer,
        guest: '',
        id: uuidv4().substr(0, 4),
        pos: { x: 0, y: 0 },
        logs: [],
    }
    globalSessions[session.id] = session
    return session
}

export const getSession = (id: string) => {
    return globalSessions[id] ?? null
}
