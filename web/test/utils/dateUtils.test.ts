import { describe, it, expect } from 'vitest'
import { formatDisplayDate, toDateInputValue, isValidDate } from '../../features/TaskManagement/utils/dateUtils'

describe('dateUtils', () => {
  describe('formatDisplayDate', () => {
    it('should format a valid date string correctly', () => {
      const result = formatDisplayDate('2024-01-15T10:30:00Z')
      expect(result).toMatch(/\d+\/\d+\/2024/) // Matches actual format: MM/DD/YYYY
    })

    it('should format a Date object correctly', () => {
      const date = new Date('2024-01-15T10:30:00Z')
      const result = formatDisplayDate(date)
      expect(result).toMatch(/\d+\/\d+\/2024/) // Matches actual format: MM/DD/YYYY
    })

    it('should return "Invalid Date" for invalid input', () => {
      const result = formatDisplayDate('invalid-date')
      expect(result).toBe('Invalid Date') // Matches actual behavior
    })

    it('should handle null input', () => {
      const result = formatDisplayDate(null)
      expect(result).toBe('Not set') // Matches actual behavior
    })

    it('should handle undefined input', () => {
      const result = formatDisplayDate(undefined)
      expect(result).toBe('Not set') // Matches actual behavior
    })
  })

  describe('toDateInputValue', () => {
    it('should convert Date to input value string', () => {
      const date = new Date('2024-01-15T10:30:00Z')
      const result = toDateInputValue(date)
      expect(result).toBe('2024-01-15')
    })

    it('should convert date string to input value string', () => {
      const result = toDateInputValue('2024-01-15T10:30:00Z')
      expect(result).toBe('2024-01-15')
    })

    it('should return empty string for invalid date', () => {
      const result = toDateInputValue('invalid-date')
      expect(result).toBe('')
    })
  })

  describe('isValidDate', () => {
    it('should return true for valid date string', () => {
      const result = isValidDate('2024-01-15')
      expect(result).toBe(true)
    })

    it('should return true for valid Date object', () => {
      const date = new Date('2024-01-15')
      const result = isValidDate(date)
      expect(result).toBe(true)
    })

    it('should return false for invalid date string', () => {
      const result = isValidDate('invalid-date')
      expect(result).toBe(false)
    })

    it('should return false for null', () => {
      const result = isValidDate(null)
      expect(result).toBe(false)
    })

    it('should return false for undefined', () => {
      const result = isValidDate(undefined)
      expect(result).toBe(false)
    })
  })
})
